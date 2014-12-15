Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class GestionAlumno

    Dim sql As String
    Dim cm As MySqlCommand
    Dim dr As MySqlDataReader
    Dim ingresar As Boolean
    Dim modificar As Boolean

    Dim cn As MySqlConnection = New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")
    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        QObtieneCursos()
    End Sub

    'Query para Obtener los Cursos
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

    'Procedimiento que obtiene todos los alumnos de un Curso
    Private Sub QObtieneAlumnos(ByVal Curso As String)
        Try

            cn.Open()
            sql = "SELECT rut FROM alumno WHERE curso_id='" & Curso & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            While dr.Read()
                ListBox2.Items.Add(dr(0))
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Procedimiento que obtiene todos Datos de un alumno de un Curso
    Private Sub QObtieneDatosAlumno(ByVal RutAlumno As String)
        Try

            cn.Open()
            sql = "SELECT nombre, usuario_rut FROM alumno WHERE rut='" & RutAlumno & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            dr.Read()
            TextBox2.Text = dr(0)

            'ComboBox1.Items.Add(dr(1))
            ComboBox1.Text = dr(1)
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        GroupBox1.Enabled = False
        TextBox1.Clear()
        TextBox2.Clear()

        ComboBox1.Items.Clear()
        Dim Curso As String
        Curso = ListBox1.SelectedItem.ToString
        ListBox2.Items.Clear()
        QObtieneAlumnos(Curso)
        ComboBox1.Text = ""
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        GroupBox1.Enabled = False
        TextBox1.Text = ListBox2.SelectedItem.ToString
        QObtieneDatosAlumno(ListBox2.SelectedItem.ToString)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.SelectedItem <> Nothing Then
            If ListBox2.SelectedItem <> Nothing Then
                GroupBox1.Enabled = True
                modificar = True
                ingresar = False
                ComboBox1.Items.Clear()
                QObtieneApoderados()
            Else
                MessageBox.Show("Seleccione un Alumno", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            End If

        Else
            MessageBox.Show("Seleccione un Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'al presionar BOTON INGRESAR
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ListBox1.SelectedItem <> Nothing Then

            GroupBox1.Enabled = True
            TextBox1.Clear()
            TextBox2.Clear()

            ComboBox1.Items.Clear()
            ComboBox1.Text = ""

            ingresar = True
            modificar = False
            ComboBox1.Items.Clear()
            QObtieneApoderados()
        Else
            MessageBox.Show("Seleccione un Curso", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'Procedimiento que realiza una Query que obtiene todos los Apoderados
    Private Sub QObtieneApoderados()
        Try

            cn.Open()
            sql = "SELECT rut FROM usuario WHERE tipo_usuario_id=2 or tipo_usuario_id=3"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            While dr.Read()
                ComboBox1.Items.Add(dr(0))
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Procedimiento que realiza Query para Ingresar un Alumno a un Curso
    Private Sub QIngresaAlumno()
        Try
            cn.Open()
            sql = " INSERT INTO alumno (rut,nombre, usuario_rut, curso_id) VALUES ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & ComboBox1.SelectedItem.ToString & "','" & ListBox1.SelectedItem.ToString & "')"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Alumno Ingresado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show("EL alumno ya existe en el Curso, ingrese otro Alumno", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Procedimiento que realiza Query para Modificar un Alumno de un Curso
    Private Sub QModificaAlumno()

        Try
            cn.Open()
            sql = " UPDATE alumno SET rut='" & TextBox1.Text & "', nombre='" & TextBox2.Text & "', usuario_rut='" & ComboBox1.Text & "' WHERE rut='" & ListBox2.SelectedItem.ToString & "' "
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Alumno Modificado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Procedimiento que realiza Query para Eliminar un Alumno
    Private Sub QEliminaAlumno()

        Dim respuesta = MessageBox.Show("¿Está seguro de eliminar el Alumno seleccionado?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        'Dim ID As String = QRetornaIdAsignatura()
        If respuesta = Windows.Forms.DialogResult.Yes Then
            Try
                cn.Open()
                sql = " DELETE FROM alumno WHERE rut='" & ListBox2.SelectedItem.ToString & "' "
                cm = New MySqlCommand()
                cm.CommandText = sql
                cm.CommandType = CommandType.Text
                cm.Connection = cn
                cm.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Alumno eliminado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

                'Se actualiza el listbox que muestra los alumnos de un curso
                ListBox2.Items.Clear()
                QObtieneAlumnos(ListBox1.SelectedItem.ToString)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                cn.Close()
            End Try
        End If
    End Sub

    'al Presionar BOTON ELIMINAR
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListBox2.SelectedItem <> Nothing Then
            'Se realiza un DELETE de un Alumno
            QEliminaAlumno()

            ingresar = False
            modificar = False

            TextBox1.Clear()
            TextBox2.Clear()
            ComboBox1.Items.Clear()
            ComboBox1.Text = ""
        Else
            MessageBox.Show("Seleccione una Alumno", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'Funcion que comprueba si los campos Texbox están vacios
    Private Function CamposLlenos() As Boolean
        Dim Llenos As Boolean = True
        If TextBox1.Text = "" Then
            Llenos = False
        End If
        If TextBox2.Text = "" Then
            Llenos = False
        End If
        If ComboBox1.Text = "" Then
            Llenos = False
        End If

        Return Llenos
    End Function

    Private Sub ButtonGuardar_Click(sender As Object, e As EventArgs) Handles ButtonGuardar.Click

        If CamposLlenos() = True Then
            Dim respuesta = MessageBox.Show("¿Está seguro de Guardar?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            If respuesta = Windows.Forms.DialogResult.Yes Then

                GroupBox1.Enabled = False

                'Se realiza un INSERT de una asignatura
                If ingresar = True Then
                    QIngresaAlumno()
                End If

                'Se realiza un UPDATE de una asignatura
                If modificar = True Then
                    QModificaAlumno()
                End If

                ingresar = False
                modificar = False

                TextBox1.Clear()
                TextBox2.Clear()
                ComboBox1.Text = ""
                'Se actualiza el listbox que muestra las asignaturas
                ListBox2.Items.Clear()
                ComboBox1.Items.Clear()

                QObtieneAlumnos(ListBox1.SelectedItem.ToString)

            End If
        Else
            MessageBox.Show("Ingrese todos los datos del Alumno", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    
End Class