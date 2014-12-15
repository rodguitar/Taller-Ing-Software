Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class GestionComunicacion
    Dim sql As String
    Dim cm As MySqlCommand
    Dim dr As MySqlDataReader
    Dim ingresar As Boolean
    Dim modificar As Boolean

    Dim cn As MySqlConnection = New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")
    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        QObtieneCursos()
        DateTimePicker1.Value = Today
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

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        GroupBox1.Enabled = False
        CheckBox1.CheckState = CheckState.Unchecked
       
        ListBox2.Items.Clear()
        QObtieneAlumnos(ListBox1.SelectedItem.ToString)

    End Sub

    'Procedimiento que obtiene todos los alumnos de un Curso
    Private Sub QObtieneAlumnos(ByVal Curso As String)
        Try

            cn.Open()
            sql = "SELECT nombre FROM alumno WHERE curso_id='" & Curso & "'"
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

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        GroupBox1.Enabled = True
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = False And ListBox2.Items.Count > 0 Then
            ListBox2.SelectionMode = SelectionMode.One
            ListBox2.SelectedIndices.Add(0)
        ElseIf ListBox2.Items.Count > 0 Then
            ListBox2.SelectionMode = SelectionMode.MultiSimple

            'Seleccion de todo el contenido del listbox2
            Dim posicion As Long
            For posicion = 0 To ListBox2.Items.Count - 1
                ListBox2.SelectedIndices.Add(posicion)
            Next posicion
        End If

    End Sub

    Private Sub ButtonGuardar_Click(sender As Object, e As EventArgs) Handles ButtonGuardar.Click

        If RichTextBox1.Text = "" Then
            MessageBox.Show("Ingrese el texto de la Comunicación", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        Else
            If CheckBox1.Checked = False Then

                QIngresaComunicacion(RichTextBox1.Text, DateTimePicker1.Text, QobtieneRutAlmuno(ListBox2.SelectedItem.ToString), ListBox1.SelectedItem.ToString)
                MessageBox.Show("Comunicación Ingresada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
            Else
                Dim F As Long
                Dim PosicionAlumSelec As String
                For F = 0 To ListBox2.SelectedItems.Count - 1
                    PosicionAlumSelec = ListBox2.SelectedIndices.Item(F)
                    QIngresaComunicacion(RichTextBox1.Text, DateTimePicker1.Text, QobtieneRutAlmuno(ListBox2.Items(PosicionAlumSelec)), ListBox1.SelectedItem.ToString)
                Next F
                MessageBox.Show("Comunicaciones Ingresadas Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
            End If
            ListBox1.Items.Clear()
            ListBox2.Items.Clear()
            QObtieneCursos()
            RichTextBox1.Clear()
            GroupBox1.Enabled = False

        End If


    End Sub

    'función que realiza query para obtener el rut del nombre de un almuno
    Private Function QobtieneRutAlmuno(ByVal Nombre As String)
        Dim RutAlumno As String
        Try

            cn.Open()
            sql = "SELECT rut FROM alumno WHERE nombre='" & Nombre & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()

            dr.Read()
            RutAlumno = dr(0)

            dr.Close()
            cn.Close()

            Return RutAlumno
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return ""
        End Try

    End Function

    'Procedimiento que realiza Query para Ingresar una comunicacion a un alumno
    Private Sub QIngresaComunicacion(ByVal Desc As String, ByVal Fecha As String, ByVal alum As String, ByVal Curso As String)
        Try
            cn.Open()
            sql = " INSERT INTO comunicacion (descripcion,fecha, alumno_rut, curso_id) VALUES ('" & Desc & "',STR_TO_DATE('" & Fecha & "', '%d/%m/%Y'),'" & alum & "','" & Curso & "')"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    
End Class