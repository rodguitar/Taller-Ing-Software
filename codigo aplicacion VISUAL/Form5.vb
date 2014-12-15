Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class GestionAnotacion

    Dim sql As String
    Dim cm As MySqlCommand
    Dim dr As MySqlDataReader
    Dim ingresar As Boolean
    Dim modificar As Boolean
    Dim cn As MySqlConnection = New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")

    Dim IngresaSoloAsig As Boolean
    Dim IngresaSoloAlum As Boolean
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Enabled = False
        Label3.Enabled = False
        ComboBox2.Enabled = False
        ComboBox3.Enabled = False
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
                ComboBox1.Items.Add(dr(0))
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim Curso As String
        Curso = ComboBox1.SelectedItem.ToString
        ComboBox2.Items.Clear()
        ComboBox2.Text = Nothing
        ComboBox3.Items.Clear()
        ComboBox3.Text = Nothing
        ListView1.Clear()
        QObtieneAsignaturas(Curso)
        QObtieneAlumnos(Curso)
        Label2.Enabled = True
        Label3.Enabled = True
        ComboBox2.Enabled = True
        ComboBox3.Enabled = True
        GroupBox1.Enabled = False
        IngresaSoloAlum = False
        IngresaSoloAsig = False

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
                ComboBox3.Items.Add(dr(0))
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
            sql = "SELECT nombre FROM asignatura WHERE curso_id='" & Curso & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            While dr.Read()
                ComboBox2.Items.Add(dr(0))
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox2.SelectedItem = Nothing Then
            QObtieneAnotacionesAlum(ComboBox3.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
            IngresaSoloAlum = True
            IngresaSoloAsig = False
        Else
            QObtieneAnotacionesAsigAlum(ComboBox3.SelectedItem.ToString, ComboBox2.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
            IngresaSoloAlum = True
            IngresaSoloAsig = True
        End If

        GroupBox1.Enabled = False
    End Sub

    'Procedimiento que obtiene todas las anotaciones de un alumno seleccionado
    Private Sub QObtieneAnotacionesAlum(ByVal NombreAlum As String, ByVal CursoID As String)
        Try

            cn.Open()
            sql = "SELECT anot.id, asig.nombre, ti.tipo, DATE_FORMAT(anot.fecha,'%d/%m/%Y') as Fecha FROM anotacion anot, alumno al, tipo_anotacion ti, asignatura asig WHERE ti.id=anot.tipo_anotacion_id and al.rut=anot.alumno_rut and asig.id=anot.asignatura_id and al.nombre= '" & NombreAlum & "' and al.curso_id='" & CursoID & "' ORDER BY anot.fecha"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()

            ListView1.Clear()
            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(0).Text = "ID"
            ListView1.Columns(0).Width = 50

            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(1).Text = "Asignatura"
            ListView1.Columns(1).Width = 80

            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(2).Text = "Tipo"
            ListView1.Columns(2).Width = 80

            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(3).Text = "Fecha"
            ListView1.Columns(3).Width = 80

            Dim fila As Integer = 0
            While dr.Read()

                ListView1.Items.Add(dr(0))
                ListView1.Items.Item(fila).SubItems.Add(dr(1))
                ListView1.Items.Item(fila).SubItems.Add(dr(2))
                ListView1.Items.Item(fila).SubItems.Add(dr(3))
                fila = fila + 1
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Procedimiento que obtiene todas las anotaciones de una asignatura seleccionada y un Alumno Seleccionado
    Private Sub QObtieneAnotacionesAsigAlum(ByVal NombreAlum As String, ByVal NombreAsig As String, ByVal CursoID As String)
        Try

            cn.Open()
            sql = "SELECT anot.id, ti.tipo, DATE_FORMAT(anot.fecha,'%d/%m/%Y') as Fecha FROM anotacion anot, alumno al, asignatura asig, tipo_anotacion ti WHERE ti.id=anot.tipo_anotacion_id and anot.alumno_rut= al.rut and al.nombre=  '" & NombreAlum & "' and asig.id=anot.asignatura_id and asig.nombre=  '" & NombreAsig & "' and al.curso_id= '" & CursoID & "' ORDER BY anot.fecha"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()

            ListView1.Clear()
            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(0).Text = "ID"
            ListView1.Columns(0).Width = 50

            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(1).Text = "Tipo"
            ListView1.Columns(1).Width = 80

            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(2).Text = "Fecha"
            ListView1.Columns(2).Width = 80

            Dim fila As Integer = 0
            While dr.Read()

                ListView1.Items.Add(dr(0))
                ListView1.Items.Item(fila).SubItems.Add(dr(1))
                ListView1.Items.Item(fila).SubItems.Add(dr(2))
                fila = fila + 1
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Procedimiento que obtiene todas las anotaciones de una asignatura seleccionada
    Private Sub QObtieneAnotacionesAsig(ByVal Nombre As String, ByVal CursoID As String)
        Try

            cn.Open()
            sql = "SELECT anot.id, al.nombre, ti.tipo, DATE_FORMAT(anot.fecha,'%d/%m/%Y') as Fecha FROM anotacion anot, alumno al , tipo_anotacion ti, asignatura asig WHERE ti.id=anot.tipo_anotacion_id and anot.alumno_rut= al.rut and asig.id=anot.asignatura_id and asig.nombre= '" & Nombre & "' and al.curso_id= '" & CursoID & "' ORDER BY anot.fecha"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()

            ListView1.Clear()
            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(0).Text = "ID"
            ListView1.Columns(0).Width = 50

            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(1).Text = "Alumno"
            ListView1.Columns(1).Width = 120

            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(2).Text = "Tipo"
            ListView1.Columns(2).Width = 80

            ListView1.Columns.Add(New ColumnHeader)
            ListView1.Columns(3).Text = "Fecha"
            ListView1.Columns(3).Width = 80

            Dim fila As Integer = 0
            While dr.Read()

                ListView1.Items.Add(dr(0))
                ListView1.Items.Item(fila).SubItems.Add(dr(1))
                ListView1.Items.Item(fila).SubItems.Add(dr(2))
                ListView1.Items.Item(fila).SubItems.Add(dr(3))
                fila = fila + 1
            End While
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox3.SelectedItem = Nothing Then

            QObtieneAnotacionesAsig(ComboBox2.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
            IngresaSoloAsig = True
            IngresaSoloAlum = False
        Else
            QObtieneAnotacionesAsigAlum(ComboBox3.SelectedItem.ToString, ComboBox2.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
            IngresaSoloAlum = True
            IngresaSoloAsig = True
        End If
        GroupBox1.Enabled = False
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        GroupBox1.Enabled = False
        RichTextBox1.Text = QObtieneDescripcion(ListView1.FocusedItem.SubItems(0).Text)
        
        If ComboBox2.SelectedItem = Nothing Or ComboBox3.SelectedItem = Nothing Then
            If ListView1.FocusedItem.SubItems(2).Text = "Positiva" Then
                CheckBox1.Checked = True
            Else
                CheckBox2.Checked = True
            End If
            DateTimePicker1.Value = ListView1.FocusedItem.SubItems(3).Text
        Else
            If ListView1.FocusedItem.SubItems(1).Text = "Positiva" Then
                CheckBox1.Checked = True
            Else
                CheckBox2.Checked = True
            End If
            DateTimePicker1.Value = ListView1.FocusedItem.SubItems(2).Text
        End If

    End Sub

    'Funcion que realiza Query para obtener la descripcion de una Anotacion
    Private Function QObtieneDescripcion(ByVal ID_Anotacion As String)
        Dim ID_anot As String = ""
        Try

            cn.Open()
            sql = "SELECT descripcion FROM anotacion WHERE id='" & ID_Anotacion & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()

            dr.Read()
            ID_anot = dr(0)

            dr.Close()
            cn.Close()

            Return ID_anot
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return ""
        End Try
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ComboBox1.SelectedItem <> Nothing Then
            If ComboBox2.SelectedItem <> Nothing Then
                If ComboBox3.SelectedItem <> Nothing Then
                    GroupBox1.Enabled = True
                    RichTextBox1.Clear()
                    CheckBox1.Checked = False
                    CheckBox2.Checked = False
                    ingresar = True
                    modificar = False
                    DateTimePicker1.Value = Today
                Else
                    MessageBox.Show("Seleccione un Alumno", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End If
            Else
                MessageBox.Show("Seleccione una Asignatura", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            End If

        Else
            MessageBox.Show("Seleccione un Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If


    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        CheckBox2.Checked = False

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged

        CheckBox1.Checked = False

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ComboBox1.SelectedItem <> Nothing Then
            If ComboBox2.SelectedItem <> Nothing Or ComboBox3.SelectedItem <> Nothing Then
                If ListView1.SelectedItems.Count <> Nothing Then
                    GroupBox1.Enabled = True
                    modificar = True
                    ingresar = False
                Else
                    MessageBox.Show("Seleccione una Anotación", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End If
            Else
                MessageBox.Show("Seleccione una Asignatura o un Alumno", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            End If

        Else
            MessageBox.Show("Seleccione un Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If


    End Sub

    'Procedimiento que realiza Query para Eliminar una Anotacion a partir del ID
    Private Sub QEliminaAnotacion(ByVal IDAnotacion As String)

        Dim respuesta = MessageBox.Show("¿Está seguro de eliminar la Anotación seleccionada?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        'Dim ID As String = QRetornaIdAsignatura()
        If respuesta = Windows.Forms.DialogResult.Yes Then
            Try
                cn.Open()
                sql = " DELETE FROM anotacion WHERE id='" & IDAnotacion & "' "
                cm = New MySqlCommand()
                cm.CommandText = sql
                cm.CommandType = CommandType.Text
                cm.Connection = cn
                cm.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Anotación eliminada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

            Catch ex As Exception
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                cn.Close()
            End Try
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ComboBox1.SelectedItem <> Nothing Then
            If ComboBox2.SelectedItem <> Nothing Or ComboBox3.SelectedItem <> Nothing Then
                If ListView1.SelectedItems.Count <> Nothing Then
                    
                    ingresar = False
                    modificar = False
                    'Metodo para eliminar una ANOTACION
                    QEliminaAnotacion(ListView1.FocusedItem.SubItems(0).Text)

                    RichTextBox1.Clear()
                    CheckBox1.Checked = False
                    CheckBox1.Checked = False
                    ListView1.Clear()
                    If IngresaSoloAlum = True And IngresaSoloAsig = True Then
                        QObtieneAnotacionesAsigAlum(ComboBox3.SelectedItem.ToString, ComboBox2.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
                    Else
                        If IngresaSoloAsig = True Then
                            QObtieneAnotacionesAsig(ComboBox2.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
                        Else
                            QObtieneAnotacionesAlum(ComboBox3.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
                        End If
                    End If

                Else
                    MessageBox.Show("Seleccione una Anotación", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End If
            Else
                MessageBox.Show("Seleccione una Asignatura o un Alumno", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            End If

        Else
            MessageBox.Show("Seleccione un Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'Funcion que comprueba si todos los campos están completados
    Private Function CamposLlenos() As Boolean
        Dim Llenos As Boolean = True
        If RichTextBox1.Text = "" Then
            Llenos = False
        End If
        If CheckBox1.Checked = False And CheckBox2.Checked = False Then
            Llenos = False
        End If
        If DateTimePicker1.Text = "" Then
            Llenos = False
        End If

        Return Llenos
    End Function

    'función que realiza query para obtener el ID de una asignatura a partir de un CURSO y NOMBRE de la asignatura
    Private Function QobtieneIdAsig(ByVal Curso As String, ByVal Nombre As String)
        Dim RutAlumno As String
        Try

            cn.Open()
            sql = "SELECT id FROM asignatura WHERE nombre='" & Nombre & "' and curso_id='" & Curso & "' "
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

    'Procedimiento que realiza Query para Ingresar una Anotacion a un Alumno en una Asignatura
    Private Sub QIngresaAnotacion(ByVal Fecha As String, TipoAnot As String, RutAlum As String, ByVal AsigID As String)
        Try
            cn.Open()
            sql = " INSERT INTO anotacion (descripcion,fecha, tipo_anotacion_id, alumno_rut,asignatura_id) VALUES ('" & RichTextBox1.Text & "', STR_TO_DATE('" & Fecha & "', '%d/%m/%Y'),'" & TipoAnot & "' ,'" & RutAlum & "','" & AsigID & "')"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Anotación Ingresada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Procedimiento que realiza Query para Modificar una Anotación de un alumno a partir del ID
    Private Sub QModificaAlumno(ByVal Desc As String, ByVal Fecha As String, ByVal Tipo As String, ByVal AnotacionID As String)

        Try
            cn.Open()
            sql = " UPDATE anotacion SET descripcion='" & Desc & "', fecha= STR_TO_DATE('" & Fecha & "', '%d/%m/%Y'), tipo_anotacion_id='" & Tipo & "' WHERE id='" & AnotacionID & "' "
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Anotación Modificada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If CamposLlenos() = True Then
            Dim respuesta = MessageBox.Show("¿Está seguro de Guardar?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            If respuesta = Windows.Forms.DialogResult.Yes Then

                GroupBox1.Enabled = False

                'Se realiza un INSERT de una asignatura
                If ingresar = True Then
                    If CheckBox1.Checked = True Then ' Si es Positiva se realiza el IF
                        Dim rutAl = QobtieneRutAlmuno(ComboBox3.SelectedItem.ToString)
                        Dim AsigID = QobtieneIdAsig(ComboBox1.SelectedItem.ToString, ComboBox2.SelectedItem.ToString)
                        QIngresaAnotacion(DateTimePicker1.Text, "1", rutAl, AsigID)
                    Else 'Si es Negativa se realiza el ELSE
                        Dim rutAl = QobtieneRutAlmuno(ComboBox3.SelectedItem.ToString)
                        Dim AsigID = QobtieneIdAsig(ComboBox1.SelectedItem.ToString, ComboBox2.SelectedItem.ToString)
                        QIngresaAnotacion(DateTimePicker1.Text, "2", rutAl, AsigID)
                    End If

                End If

                'Se realiza un UPDATE de una asignatura
                If modificar = True Then
                    Dim ID = ListView1.FocusedItem.SubItems(0).Text
                    If CheckBox1.Checked = True Then
                        QModificaAlumno(RichTextBox1.Text, DateTimePicker1.Text, "1", ID)
                    Else
                        QModificaAlumno(RichTextBox1.Text, DateTimePicker1.Text, "2", ID)
                    End If

                End If

                RichTextBox1.Clear()
                CheckBox1.Checked = False
                CheckBox1.Checked = False
                ListView1.Clear()

                'Se actualiza el listview que muestra las anotaciones

                If IngresaSoloAlum = True And IngresaSoloAsig = True Then
                    QObtieneAnotacionesAsigAlum(ComboBox3.SelectedItem.ToString, ComboBox2.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
                Else
                    If IngresaSoloAsig = True Then
                        QObtieneAnotacionesAsig(ComboBox2.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
                    Else
                        QObtieneAnotacionesAlum(ComboBox3.SelectedItem.ToString, ComboBox1.SelectedItem.ToString)
                    End If
                End If
                ingresar = False
                modificar = False

            End If
        Else
            MessageBox.Show("Ingrese todos los datos de la Anotación", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub
End Class