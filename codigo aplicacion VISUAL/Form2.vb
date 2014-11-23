Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class GestionCurso
    Dim sql As String
    Dim cm As MySqlCommand
    Dim dr As MySqlDataReader
    Dim ingresar As Boolean
    Dim modificar As Boolean

    Dim cn As MySqlConnection = New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        QObtieneCursos()

    End Sub

    'Query para seleccionar los Cursos
    Private Sub QObtieneCursos()
        Try

            cn.Open()
            sql = "SELECT id from curso"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            While dr.Read()
                ListBox1.Items.Add(dr(0))
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GroupBox1.Enabled = True
        TextBox1.Clear()
        ingresar = True
        modificar = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If ListBox1.SelectedItem <> Nothing Then
            GroupBox1.Enabled = True
            modificar = True
            ingresar = False
        Else
            MessageBox.Show("Seleccione un Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If

    End Sub

    'Al presionar GUARDAR se realiza un "INSERT" o "UPDATE" de un Curso en la BD
    Private Sub ButtonGuardar_Click(sender As Object, e As EventArgs) Handles ButtonGuardar.Click

        Dim respuesta = MessageBox.Show("¿Está seguro de Guardar?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        If respuesta = Windows.Forms.DialogResult.Yes And TextBox1.Text <> "" Then

            GroupBox1.Enabled = False

            'Se realiza un INSERT de un curso
            If ingresar = True Then
                QIngresaCurso()
            End If

            'Se realiza un UPDATE de un curso
            If modificar = True Then
                QModificaCurso()
            End If

            ingresar = False
            modificar = False

            TextBox1.Clear()
            'Se actualiza el listbox que muestra los cursos
            ListBox1.Items.Clear()

            QObtieneCursos()

        ElseIf respuesta = Windows.Forms.DialogResult.Yes And TextBox1.Text = "" Then

            MessageBox.Show("Ingrese Código del Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

        End If
    End Sub

    'Query para Ingresar un Curso
    Private Sub QIngresaCurso()
        Try
            cn.Open()
            sql = " INSERT INTO curso (id) VALUES ('" & TextBox1.Text & "')"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Curso Ingresado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show("El Curso ya existe, ingrese otro Curso", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Query para Modificar un Curso
    Private Sub QModificaCurso()
        Try
            cn.Open()
            sql = " UPDATE curso SET id='" & TextBox1.Text & "' WHERE id='" & ListBox1.SelectedItem.ToString & "' "
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Curso Modificado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Query para Eliminar un Curso
    Private Sub QEliminaCurso()

        Dim respuesta = MessageBox.Show("¿Está seguro de eliminar el Curso seleccionado?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        If respuesta = Windows.Forms.DialogResult.Yes Then
            Try
                cn.Open()
                sql = " DELETE FROM curso where id='" & ListBox1.SelectedItem.ToString & "' "
                cm = New MySqlCommand()
                cm.CommandText = sql
                cm.CommandType = CommandType.Text
                cm.Connection = cn
                cm.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Curso Eliminado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

                'Se actualiza el listbox que muestra los cursos
                ListBox1.Items.Clear()
                QObtieneCursos()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                cn.Close()
            End Try
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        GroupBox1.Enabled = False
        TextBox1.Text = ListBox1.SelectedItem.ToString
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListBox1.SelectedItem <> Nothing Then
            'Se realiza un DELETE de un Curso
            QEliminaCurso()

            ingresar = False
            modificar = False

            TextBox1.Clear()

        Else
            MessageBox.Show("Seleccione un Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub
End Class