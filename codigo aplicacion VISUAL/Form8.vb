Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class GestionEvaluacion
    Dim sql As String
    Dim cm As MySqlCommand
    Dim dr As MySqlDataReader
    Dim ingresar As Boolean
    Dim modificar As Boolean
    Dim MatrizRutNombres(,) As String

    Dim da As MySqlDataAdapter
    Dim dt As DataTable

    Dim cn As MySqlConnection = New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")



    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button1.Enabled = False
        QObtieneCursos()

        DateTimePicker1.Value = "01/03/" & Today.Year
        DateTimePicker2.Value = "31/12/" & Today.Year

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
        Panel1.Controls.Clear()
        DataGridView1.Enabled = False
        ' Panel1.Controls.Clear()
        ComboBox2.Items.Clear()
        ComboBox3.Items.Clear()
        ComboBox2.Text = Nothing
        ComboBox2.Enabled = True
        GroupBox1.Enabled = False
        Button1.Enabled = False
        ComboBox3.Enabled = False
        Label1.Enabled = True
        Label6.Enabled = False
        QObtieneAsignaturas(ComboBox1.SelectedItem.ToString)
        QObtieneAlumnos(ComboBox1.SelectedItem.ToString)
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

    'Funcion que obtiene la cantidad de alumnos de un curso
    Private Function QObtieneCantAlumnos(ByVal Curso As String)
        Dim cantidad As Integer
        Try

            cn.Open()
            sql = "SELECT COUNT(rut) FROM alumno WHERE curso_id='" & Curso & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            dr.Read()
            cantidad = dr(0)

            dr.Close()
            cn.Close()
            Return cantidad
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return 0
        End Try
    End Function

    'Funcion que obtiene los nombres de los alumnos de un curso
    Private Function QObtieneNombresAlum(ByVal Curso As String, ByVal Tamano As Integer)
        Dim nombres(Tamano) As String

        Try

            cn.Open()
            sql = "SELECT nombre FROM alumno WHERE curso_id='" & Curso & "' ORDER BY nombre"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            Dim i As Integer = 0
            While dr.Read()
                nombres(i) = dr(0)
                i = i + 1
            End While

            dr.Close()
            cn.Close()
            Return nombres
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return ""
        End Try
    End Function

    'Funcion que obtiene la Cantidad Maxima de notas de un alumno entre todo el Curso
    Private Function QObtieneMaxCant(ByVal AsigID As String, ByVal FechaIni As Date, ByVal FechaFin As Date)
        Dim cantidad As Integer = 0
        Try

            cn.Open()
            sql = "SELECT COUNT(n.alumno_rut) FROM nota n, alumno al, asignatura asig WHERE al.rut=n.alumno_rut AND asig.id=n.asignatura_id AND asig.id='" & AsigID & "' and n.fecha BETWEEN STR_TO_DATE('" & FechaIni & "', '%d/%m/%Y')  AND STR_TO_DATE('" & FechaFin & "', '%d/%m/%Y') GROUP BY n.alumno_rut"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()

            While dr.Read()
                If cantidad < dr(0) Then
                    cantidad = dr(0)
                End If
            End While

            dr.Close()
            cn.Close()
            Return cantidad
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return 0
        End Try
    End Function

    'Funcion que obtiene la cantidad de notas que existen en una asignatura
    Private Function QObtieneCantNotas(ByVal AsigID As String, ByVal FechaIni As Date, ByVal FechaFin As Date)
        Dim cantidad As Integer = 0
        Try

            cn.Open()
            sql = "SELECT COUNT(n.valor) FROM nota n, alumno al, asignatura asig WHERE al.rut=n.alumno_rut AND asig.id=n.asignatura_id AND asig.id='" & AsigID & "' and n.fecha BETWEEN STR_TO_DATE('" & FechaIni & "', '%d/%m/%Y')  AND STR_TO_DATE('" & FechaFin & "', '%d/%m/%Y')"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()

            dr.Read()

            cantidad = dr(0)

            dr.Close()
            cn.Close()
            Return cantidad
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return 0
        End Try
    End Function

    Private Sub QMuetraNotas(ByVal AsigID As String, ByVal LargoCurso As Integer, ByVal NombresAlum() As String, ByVal CantNotasMax As Integer, ByVal CantidadNotas As Integer, ByVal FechaIni As Date, ByVal FechaFin As Date)
        Dim MatrizAux(CantidadNotas - 1, 1) As String
        Try

            cn.Open()
            sql = "SELECT al.nombre, n.valor FROM nota n, alumno al, asignatura asig WHERE al.rut=n.alumno_rut AND asig.id=n.asignatura_id AND asig.id='" & AsigID & "' and n.fecha BETWEEN STR_TO_DATE('" & FechaIni & "', '%d/%m/%Y')  AND STR_TO_DATE('" & FechaFin & "', '%d/%m/%Y') ORDER BY n.fecha"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()

            Dim j As Integer = 0
            While dr.Read()

                MatrizAux(j, 0) = dr(0)
                MatrizAux(j, 1) = dr(1)
                j = j + 1
            End While

            For i = 0 To CantNotasMax - 1
                DataGridView1.Columns.Add("", "")
                DataGridView1.Columns(0).HeaderText = "Alumnos"

                If i = 0 Then
                    DataGridView1.Columns.Add("", "")
                    DataGridView1.Rows.Add(LargoCurso)
                    For aux = 0 To LargoCurso - 1

                        DataGridView1.Item(0, aux).Value = NombresAlum(aux)
                    Next

                End If
            Next

            For NumColum = 1 To CantNotasMax
                DataGridView1.Columns(NumColum).HeaderText = "Nota " & (NumColum).ToString
                'DataGridView1.Columns(NumColum).HeaderCell.Value = "Nota " & (NumColum).ToString
            Next

            For aux2 = 0 To LargoCurso - 1
                Dim SuNota As Integer = 1
                For aux3 = 0 To CantidadNotas - 1
                    If DataGridView1.Item(0, aux2).Value = MatrizAux(aux3, 0) Then
                        DataGridView1.Item(SuNota, aux2).Value = MatrizAux(aux3, 1)
                        SuNota = SuNota + 1
                    End If
                Next
            Next

            dr.Close()

            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

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

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Panel1.Controls.Clear()
        'Panel1.Dispose()

        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        DataGridView1.Enabled = False
        GroupBox1.Enabled = True
        Button1.Enabled = True
        ComboBox3.Enabled = True
        Label6.Enabled = True


    End Sub

    'Procedimiento que obtiene todos los alumnos de un Curso
    Private Sub QObtieneAlumnos(ByVal Curso As String)
        Try

            cn.Open()
            sql = "SELECT nombre FROM alumno WHERE curso_id='" & Curso & "' ORDER BY nombre"
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DataGridView1.Enabled = True
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()


        Dim ID_Asig = QobtieneIdAsig(ComboBox1.SelectedItem.ToString, ComboBox2.SelectedItem.ToString)
        Dim CantAlumnos = QObtieneCantAlumnos(ComboBox1.SelectedItem.ToString)
        Dim NombresAlumnos = QObtieneNombresAlum(ComboBox1.SelectedItem.ToString, CantAlumnos)
        Dim MaximaCantidad = QObtieneMaxCant(ID_Asig, DateTimePicker1.Text, DateTimePicker2.Text)
        Dim CantidadNotasAsig = QObtieneCantNotas(ID_Asig, DateTimePicker1.Text, DateTimePicker2.Text)
        QMuetraNotas(ID_Asig, CantAlumnos, NombresAlumnos, MaximaCantidad, CantidadNotasAsig, DateTimePicker1.Text, DateTimePicker2.Text)
    End Sub

    'Funcion que obtiene los rut y nombres de los alumnos de un curso y los retorna en una matriz
    Private Function QObtieneRutNombresAlum(ByVal CursoID As String, ByVal TamanoCurso As Integer)
        Dim RutNombres(TamanoCurso - 1, 1) As String

        Try

            cn.Open()
            sql = "SELECT rut, nombre FROM alumno WHERE curso_id='" & CursoID & "' ORDER BY nombre"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            Dim i As Integer = 0
            While dr.Read()
                RutNombres(i, 0) = dr(0)
                RutNombres(i, 1) = dr(1)
                i = i + 1
            End While

            dr.Close()
            cn.Close()
            Return RutNombres
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return ""
        End Try
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If ComboBox1.SelectedItem <> Nothing Then
            If ComboBox2.SelectedItem <> Nothing Then

                Button3.Enabled = True
                Dim a = 3
                Dim aux As Integer = 0
                Dim CantidadAlumnos = QObtieneCantAlumnos(ComboBox1.SelectedItem.ToString)
                Dim RutNombresAlum = QObtieneRutNombresAlum(ComboBox1.SelectedItem.ToString, CantidadAlumnos)
                MatrizRutNombres = RutNombresAlum
                For i As Integer = 0 To CantidadAlumnos - 1

                    Dim NuevaLabel As New Label
                    With NuevaLabel
                        .Location = New System.Drawing.Point(38, 25 + aux)
                        .BackColor = Color.Transparent
                        .Name = "Label" & (i + 8).ToString
                        .Size = New System.Drawing.Size(120, 23)
                        .Text = RutNombresAlum(i, 1)


                    End With
                    Controls.Add(NuevaLabel)
                    Panel1.Controls.Add(NuevaLabel)

                    Dim NuevaNota As New MaskedTextBox
                    With NuevaNota
                        .Location = New System.Drawing.Point(168, 19 + aux)
                        .Mask = "99.9"
                        .Name = "MaskedTextBox" & (i + 2).ToString
                        .Size = New System.Drawing.Size(30, 20)
                        '.Text = "  0"

                    End With
                    Controls.Add(NuevaNota)
                    Panel1.Controls.Add(NuevaNota)

                    Dim NuevaFecha As New DateTimePicker
                    With NuevaFecha
                        .Format = System.Windows.Forms.DateTimePickerFormat.[Short]
                        .Location = New System.Drawing.Point(198, 19 + aux)
                        .Name = "DateTimePicker" & (i + 3).ToString
                        .Size = New System.Drawing.Size(94, 22)
                        .Value = New Date(2014, 11, 30, 0, 0, 0, 0)
                        .Text = Today.Day & "/" & Today.Month & "/" & Today.Year

                    End With

                    Controls.Add(NuevaFecha)
                    Panel1.Controls.Add(NuevaFecha)
                    If i = CantidadAlumnos - 1 Then
                        Button3.Location = New System.Drawing.Point(122, 19 + aux + 40)
                        Panel1.Controls.Add(Button3)
                        Button3.Visible = True
                    End If

                    'AddHandler NuevaNota.LostFocus, AddressOf NuevaNota_LostFocus
                    'AddHandler NuevaNota., AddressOf Button3_Click

                    aux = aux + 22
                Next
            Else
                MessageBox.Show("Seleccione una Asignatura", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            End If

        Else
            MessageBox.Show("Seleccione un Curso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If

    End Sub

    'Private Sub NuevaNota_LostFocus(ByVal sender As Object, _
    '    ByVal e As System.EventArgs)

    '    MsgBox(Controls.Item(9).Text)
    'End Sub
    Private Function CamposVacios()
        Dim Vacios As Boolean = True
        Dim CantidadAlumnos = QObtieneCantAlumnos(ComboBox1.SelectedItem.ToString)
        Dim aux As Integer = 1
        While aux <= CantidadAlumnos
            If Panel1.Controls.Item(aux).Text <> "  ." Then
                Return False
            End If
            aux = aux + 3
        End While
        Return Vacios
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If CamposVacios() = True Then
            MessageBox.Show("¡Campos Vacios! Ingrese al menos una Nota a un Alumno", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        Else
            Dim respuesta = MessageBox.Show("¿Está seguro de Ingresar las Notas?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

            If respuesta = Windows.Forms.DialogResult.Yes Then
                Dim IdAsignatura = QobtieneIdAsig(ComboBox1.SelectedItem.ToString, ComboBox2.SelectedItem.ToString)
                Dim CantidadAlumnos = QObtieneCantAlumnos(ComboBox1.SelectedItem.ToString)
                Dim aux2 As Integer = 1
                Dim aux3 As Integer = 0
                While aux2 <= CantidadAlumnos * 3
                    'If Panel1.Controls.Item(aux2).Text <> "" Then
                    QIngresaNota(Panel1.Controls.Item(aux2).Text, Panel1.Controls.Item(aux2 + 1).Text, MatrizRutNombres(aux3, 0), IdAsignatura)
                    'End If
                    aux2 = aux2 + 3
                    aux3 = aux3 + 1
                End While
                'Erase MatrizRutNombres
                MatrizRutNombres = Nothing
                Panel1.Controls.Clear()
                Button3.Visible = False
                Button3.Enabled = False
                MessageBox.Show("Notas Ingresadas Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
            End If
        End If

    End Sub

    'Procedimiento que realiza Query para Ingresar una Nota a un Alumno en una Asignatura
    Private Sub QIngresaNota(ByVal ValorNota As String, ByVal Fecha As String, RutAlum As String, ByVal AsigID As String)
        Try
            cn.Open()
            sql = " INSERT INTO nota (valor,fecha, alumno_rut,asignatura_id) VALUES (FORMAT('" & ValorNota & "',1), STR_TO_DATE('" & Fecha & "', '%d/%m/%Y'),'" & RutAlum & "' ,'" & AsigID & "')"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            'MessageBox.Show("Nota Ingresada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    Friend WithEvents NotasNuevas As System.Windows.Forms.MaskedTextBox

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        GroupBox3.Enabled = False
        ListView1.Clear()
        MaskedTextBox1.Clear()
        GroupBox2.Enabled = True
        Dim ID_Asig = QobtieneIdAsig(ComboBox1.SelectedItem.ToString, ComboBox2.SelectedItem.ToString)
        QObtieneNotasAlum(ComboBox3.SelectedItem.ToString, ID_Asig, DateTimePicker1.Text, DateTimePicker2.Text)

    End Sub

    'Procedimiento que obtiene todas las Notas de un alumno seleccionado y las coloca en un listview
    Private Sub QObtieneNotasAlum(ByVal NombreAlum As String, ByVal AsigID As String, ByVal FechaIni As Date, ByVal FechaFin As Date)
        Try

            cn.Open()
            sql = "SELECT n.id, n.valor, DATE_FORMAT(n.fecha,'%d/%m/%Y') as Fecha FROM nota n, alumno al, asignatura asig WHERE al.nombre= '" & NombreAlum & "' AND al.rut=n.alumno_rut AND asig.id=n.asignatura_id AND asig.id='" & AsigID & "' AND n.fecha BETWEEN STR_TO_DATE('" & FechaIni & "', '%d/%m/%Y')  AND STR_TO_DATE('" & FechaFin & "', '%d/%m/%Y') ORDER BY n.fecha"
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
            ListView1.Columns(1).Text = "Valor"
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


    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        GroupBox3.Enabled = False
        MaskedTextBox1.Text = ListView1.FocusedItem.SubItems(1).Text
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim respuesta = MessageBox.Show("¿Está seguro de Modificar la Nota seleccionada?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        If respuesta = Windows.Forms.DialogResult.Yes Then
            QModificaNota(MaskedTextBox1.Text, ListView1.FocusedItem.SubItems(0).Text)

        End If
        MaskedTextBox1.Clear()
        Dim ID_Asig = QobtieneIdAsig(ComboBox1.SelectedItem.ToString, ComboBox2.SelectedItem.ToString)
        QObtieneNotasAlum(ComboBox3.SelectedItem.ToString, ID_Asig, DateTimePicker1.Text, DateTimePicker2.Text)
        Button6.Enabled = True
        GroupBox3.Enabled = False

    End Sub

    'Procedimiento que realiza Query para Modificar una Nota de un alumno a partir del ID
    Private Sub QModificaNota(ByVal Valor As Double, ByVal NotaID As String)

        Try
            cn.Open()
            sql = " UPDATE nota SET valor=FORMAT('" & Valor & "',1) WHERE id='" & NotaID & "' "
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Nota Modificada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

  
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        
        If ListView1.SelectedItems.Count <> Nothing Then
            Button6.Enabled = False
            GroupBox3.Enabled = True
        Else
            MessageBox.Show("Seleccione una Nota", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
          

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If ListView1.SelectedItems.Count <> Nothing Then
            QEliminaNota(ListView1.FocusedItem.SubItems(0).Text)
            MaskedTextBox1.Clear()
            Dim ID_Asig = QobtieneIdAsig(ComboBox1.SelectedItem.ToString, ComboBox2.SelectedItem.ToString)
            QObtieneNotasAlum(ComboBox3.SelectedItem.ToString, ID_Asig, DateTimePicker1.Text, DateTimePicker2.Text)
        Else
            MessageBox.Show("Seleccione una Nota", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'Procedimiento que realiza Query para Eliminar una Nota a partir del ID
    Private Sub QEliminaNota(ByVal IDNota As String)

        Dim respuesta = MessageBox.Show("¿Está seguro de eliminar la Nota seleccionada?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        If respuesta = Windows.Forms.DialogResult.Yes Then
            Try
                cn.Open()
                sql = " DELETE FROM nota WHERE id='" & IDNota & "' "
                cm = New MySqlCommand()
                cm.CommandText = sql
                cm.CommandType = CommandType.Text
                cm.Connection = cn
                cm.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Nota eliminada Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

            Catch ex As Exception
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                cn.Close()
            End Try
        End If
    End Sub
End Class