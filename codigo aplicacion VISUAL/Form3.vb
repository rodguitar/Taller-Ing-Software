Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class GestionAsignatura
    Dim sql As String
    Dim cm As MySqlCommand
    Dim dr As MySqlDataReader
    Dim ingresar As Boolean
    Dim modificar As Boolean

    Dim cn As MySqlConnection = New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

    'Procedimiento que obtiene todas las asignaturas de un Curso
    Private Sub QObtieneAsignaturas(ByVal Curso As String)
        Try

            cn.Open()
            sql = "SELECT id,nombre FROM asignatura WHERE curso_id='" & Curso & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            While dr.Read()
                ListBox2.Items.Add(dr(1))
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Funcion que retorna ID de la una Asignatura a partir de su Nombre y el Curso al que pertenece
    Private Function QRetornaIdAsignatura() As String
        Dim ID As String
        Dim Nombre As String = ListBox2.SelectedItem.ToString
        Dim Curso As String = ListBox1.SelectedItem.ToString

        Try

            cn.Open()
            sql = " SELECT id FROM asignatura WHERE nombre='" & Nombre & "' and curso_id='" & Curso & "' "
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            dr.Read()
            ID = dr(0)

            dr.Close()
            cn.Close()
            Return ID
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return ""
        End Try
    End Function


    'Query para Ingresar una asignatura
    Private Sub QIngresaAsignatura()
        Try
            cn.Open()
            sql = " INSERT INTO asignatura (nombre, curso_id) VALUES ('" & TextBox1.Text & "','" & ListBox1.SelectedItem.ToString & "')"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Asignatura Ingresada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show("La Asignatura ya existe en el Curso, ingrese otra Asignatura", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Query para Modificar una asignatura
    Private Sub QModificaAsignatura()
        Dim IdAsignatura As String = QRetornaIdAsignatura()
        Try
            cn.Open()
            sql = " UPDATE asignatura SET nombre='" & TextBox1.Text & "' WHERE id='" & IdAsignatura & "' "
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Asignatura Modificada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Query para Eliminar una asignatura
    Private Sub QEliminaAsignatura()

        Dim respuesta = MessageBox.Show("¿Está seguro de eliminar la Asignatura seleccionada?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        Dim ID As String = QRetornaIdAsignatura()
        If respuesta = Windows.Forms.DialogResult.Yes Then
            Try
                cn.Open()
                sql = " DELETE FROM asignatura WHERE id='" & ID & "' "
                cm = New MySqlCommand()
                cm.CommandText = sql
                cm.CommandType = CommandType.Text
                cm.Connection = cn
                cm.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Asignatura Eliminada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

                'Se actualiza el listbox que muestra las asignaturas
                ListBox2.Items.Clear()
                QObtieneAsignaturas(ListBox1.SelectedItem.ToString)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                cn.Close()
            End Try
        End If
    End Sub



    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        GroupBox1.Enabled = False
        TextBox1.Clear()
        Dim Curso As String
        Curso = ListBox1.SelectedItem.ToString
        ListBox2.Items.Clear()
        QObtieneAsignaturas(Curso)

    End Sub

    'al presionar el BOTON GUARDAR
    Private Sub ButtonGuardar_Click(sender As Object, e As EventArgs) Handles ButtonGuardar.Click
        Dim respuesta = MessageBox.Show("¿Está seguro de Guardar?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        If respuesta = Windows.Forms.DialogResult.Yes And TextBox1.Text <> "" Then

            GroupBox1.Enabled = False

            'Se realiza un INSERT de una asignatura
            If ingresar = True Then
                QIngresaAsignatura()
            End If

            'Se realiza un UPDATE de una asignatura
            If modificar = True Then
                QModificaAsignatura()
            End If

            ingresar = False
            modificar = False

            TextBox1.Clear()
            'Se actualiza el listbox que muestra las asignaturas
            ListBox2.Items.Clear()

            QObtieneAsignaturas(ListBox1.SelectedItem.ToString)

        ElseIf respuesta = Windows.Forms.DialogResult.Yes And TextBox1.Text = "" Then

            MessageBox.Show("Ingrese el Nombre de la Asignatura", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        GroupBox1.Enabled = False
        TextBox1.Text = ListBox2.SelectedItem.ToString
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListBox2.SelectedItem <> Nothing Then
            'Se realiza un DELETE de un Curso
            QEliminaAsignatura()

            ingresar = False
            modificar = False

            TextBox1.Clear()

        Else
            MessageBox.Show("Seleccione una Asignatura", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If ListBox1.SelectedItem <> Nothing Then

            GroupBox1.Enabled = True
            TextBox1.Clear()
            ingresar = True
            modificar = False
        Else
            MessageBox.Show("Seleccione un Curso", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.SelectedItem <> Nothing Then
            If ListBox2.SelectedItem <> Nothing Then
                GroupBox1.Enabled = True
                modificar = True
                ingresar = False
            Else
                MessageBox.Show("Seleccione una Asignatura", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            End If
            
        Else
            MessageBox.Show("Seleccione un Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub
End Class