
Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class GestionApoderado

    Dim sql As String
    Dim cm As MySqlCommand
    Dim dr As MySqlDataReader
    Dim ingresar As Boolean
    Dim modificar As Boolean

    Dim cn As MySqlConnection = New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        QObtieneApoderados()
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

    'al presionar el BOTON INGRESAR
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        GroupBox1.Enabled = True
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        ingresar = True
        modificar = False
        
    End Sub

    'al presionar el BOTON MODIFICAR
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ComboBox1.SelectedItem <> Nothing Then

            GroupBox1.Enabled = True
            modificar = True
            ingresar = False
        Else
            MessageBox.Show("Seleccione un Apoderado", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        GroupBox1.Enabled = False
        TextBox1.Text = ComboBox1.SelectedItem.ToString
        QObtieneDatosApoderado(ComboBox1.SelectedItem.ToString)
    End Sub

    'Procedimiento que obtiene todos Datos de un Apoderado
    Private Sub QObtieneDatosApoderado(ByVal RutApoderado As String)
        Try

            cn.Open()
            sql = "SELECT nombre, contrasena FROM usuario WHERE rut='" & RutApoderado & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            dr.Read()
            TextBox2.Text = dr(0)
            TextBox3.Text = dr(1)
            dr.Close()
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'funcion que realiza una Query para verificar si un Apoderado es Administrador
    Private Function EsAdministrador() As Boolean
        Dim Administrador As Boolean = True

        Try
            cn.Open()
            sql = "SELECT tipo_usuario_id FROM usuario WHERE rut='" & ComboBox1.SelectedItem.ToString & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            dr.Read()
            'comprueba si es apoderado 
            If (dr(0) = "2") Then
                Administrador = False
            End If

            dr.Close()
            cn.Close()

            Return Administrador
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return Administrador
        End Try

    End Function

    'Funcion que realiza Query para verificar si un Apoderado tiene alumnos
    Private Function QApoderadoTieneAlumnos(RutApoderado) As Boolean
        Dim Tiene As Boolean = True

        Try
            cn.Open()
            sql = "SELECT count(*) FROM alumno WHERE usuario_rut='" & RutApoderado & "'"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            dr = cm.ExecuteReader()
            dr.Read()
            'comprueba si tiene alumnos o pupilos 
            If (dr(0) = "0") Then
                Tiene = False
            End If

            dr.Close()
            cn.Close()

            Return Tiene
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return Tiene
        End Try


    End Function

    'Query para Eliminar una asignatura
    Private Sub QEliminaApoderado()

        Dim respuesta = MessageBox.Show("¿Está seguro de eliminar el Apoderado seleccionado?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        Dim RutApoderado As String = ComboBox1.SelectedItem.ToString
        If QApoderadoTieneAlumnos(RutApoderado) = False Then

            If respuesta = Windows.Forms.DialogResult.Yes And EsAdministrador() = False Then
                Try
                    cn.Open()
                    sql = " DELETE FROM usuario WHERE rut='" & RutApoderado & "' "
                    cm = New MySqlCommand()
                    cm.CommandText = sql
                    cm.CommandType = CommandType.Text
                    cm.Connection = cn
                    cm.ExecuteNonQuery()
                    cn.Close()
                    MessageBox.Show("Apoderado eliminado satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

                    'Se actualiza el combobox que muestra los apoderados
                    ComboBox1.Items.Clear()
                    QObtieneApoderados()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    cn.Close()
                End Try
            ElseIf respuesta = Windows.Forms.DialogResult.Yes And EsAdministrador() = True Then
                Try
                    cn.Open()
                    sql = " UPDATE usuario SET tipo_usuario_id='1' WHERE rut='" & RutApoderado & "' "
                    cm = New MySqlCommand()
                    cm.CommandText = sql
                    cm.CommandType = CommandType.Text
                    cm.Connection = cn
                    cm.ExecuteNonQuery()
                    cn.Close()
                    MessageBox.Show("Apoderado eliminado satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

                    'Se actualiza el combobox que muestra los apoderados
                    ComboBox1.Items.Clear()
                    QObtieneApoderados()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    cn.Close()
                End Try

            End If
        Else
            MessageBox.Show("El Apoderado tiene Alumnos asociados, primero elimine aquellos alumnos en GESTION ALUMNO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'Al presionar el BOTON ELIMINAR
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ComboBox1.SelectedItem <> Nothing Then
            'Se realiza un DELETE de un apoderado
            QEliminaApoderado()

            ingresar = False
            modificar = False

            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            ComboBox1.Text = ""

        Else
            MessageBox.Show("Seleccione un Apoderado", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'Procedimiento que realiza Query para Ingresar un Apoderado
    Private Sub QIngresaApoderado()
        Try
            cn.Open()
            sql = " INSERT INTO usuario (rut,nombre, contrasena, tipo_usuario_id) VALUES ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','2')"
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Apoderado Ingresado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show("EL Apoderado ya existe, ingrese otro Apoderado", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Procedimiento que realiza Query para Modificar un Apoderado
    Private Sub QModificaApoderado()

        Try
            cn.Open()
            sql = " UPDATE usuario SET rut='" & TextBox1.Text & "', nombre='" & TextBox2.Text & "', contrasena='" & TextBox3.Text & "' WHERE rut='" & ComboBox1.SelectedItem.ToString & "' "
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Apoderado Modificado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
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
        If TextBox3.Text = "" Then
            Llenos = False
        End If

        Return Llenos
    End Function

    'al presionar el BOTON GUARDAR 
    Private Sub ButtonGuardar_Click(sender As Object, e As EventArgs) Handles ButtonGuardar.Click
        If CamposLlenos() = True Then
            Dim respuesta = MessageBox.Show("¿Está seguro de Guardar?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            If respuesta = Windows.Forms.DialogResult.Yes Then

                GroupBox1.Enabled = False

                'Se realiza un INSERT de una asignatura
                If ingresar = True Then
                    QIngresaApoderado()
                End If

                'Se realiza un UPDATE de una asignatura
                If modificar = True Then
                    QModificaApoderado()
                End If

                ingresar = False
                modificar = False

                TextBox1.Clear()
                TextBox2.Clear()
                TextBox3.Clear()
                'Se actualiza el listbox que muestra las asignaturas
                ComboBox1.Text = ""
                ComboBox1.Items.Clear()

                QObtieneApoderados()

            End If
        Else
            MessageBox.Show("Ingrese todos los datos del Alumno", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub
End Class