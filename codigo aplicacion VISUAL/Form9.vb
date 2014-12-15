
Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class GestionAdministrador
    Dim sql As String
    Dim cm As MySqlCommand
    Dim dr As MySqlDataReader
    Dim ingresar As Boolean
    Dim modificar As Boolean

    Dim cn As MySqlConnection = New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        QObtieneAdministradores()
    End Sub
    'Procedimiento que realiza una Query que obtiene todos los Administradores
    Private Sub QObtieneAdministradores()
        Try

            cn.Open()
            sql = "SELECT rut FROM usuario WHERE tipo_usuario_id=1 or tipo_usuario_id=3"
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
        MaskedTextBox1.Clear()
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
            MessageBox.Show("Seleccione un Administrador", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        GroupBox1.Enabled = False
        MaskedTextBox1.Text = ComboBox1.SelectedItem.ToString
        QObtieneDatosAdministrador(ComboBox1.SelectedItem.ToString)
    End Sub

    'Procedimiento que obtiene todos Datos de un Administrador
    Private Sub QObtieneDatosAdministrador(ByVal RutAdministrador As String)
        Try

            cn.Open()
            sql = "SELECT nombre, contrasena FROM usuario WHERE rut='" & RutAdministrador & "'"
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

    'funcion que realiza una Query para verificar si un Administrador es Apoderado
    Private Function EsApoderado() As Boolean
        Dim Apoderado As Boolean = True

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
            If (dr(0) = "1") Then
                Apoderado = False
            End If

            dr.Close()
            cn.Close()

            Return Apoderado
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return Apoderado
        End Try

    End Function

    'funcion que realiza una Query para verificar si un usuario es solo Apoderado
    Private Function SoloApoderado() As Boolean
        Dim SoloApo As Boolean = False

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
                SoloApo = True
            End If

            dr.Close()
            cn.Close()

            Return SoloApo
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
            Return SoloApo
        End Try

    End Function

    'Funcion que realiza Query para verificar si un Administrador tiene alumnos
    Private Function QAdministradorTieneAlumnos(RutAdministrador) As Boolean
        Dim Tiene As Boolean = True

        Try
            cn.Open()
            sql = "SELECT count(*) FROM alumno WHERE usuario_rut='" & RutAdministrador & "'"
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

    'Query para Eliminar una Administrador
    Private Sub QEliminaAdministrador()

        Dim respuesta = MessageBox.Show("¿Está seguro de eliminar el Administrador seleccionado?", "Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        Dim RutAdministrador As String = ComboBox1.SelectedItem.ToString
        If QAdministradorTieneAlumnos(RutAdministrador) = False Then

            If respuesta = Windows.Forms.DialogResult.Yes And EsApoderado() = False Then
                Try
                    cn.Open()
                    sql = " DELETE FROM usuario WHERE rut='" & RutAdministrador & "' "
                    cm = New MySqlCommand()
                    cm.CommandText = sql
                    cm.CommandType = CommandType.Text
                    cm.Connection = cn
                    cm.ExecuteNonQuery()
                    cn.Close()
                    MessageBox.Show("Administrador eliminado satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

                    'Se actualiza el combobox que muestra los Administradores
                    ComboBox1.Items.Clear()
                    QObtieneAdministradores()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    cn.Close()
                End Try
            ElseIf respuesta = Windows.Forms.DialogResult.Yes And EsApoderado() = True Then
                Try
                    cn.Open()
                    sql = " UPDATE usuario SET tipo_usuario_id='2' WHERE rut='" & RutAdministrador & "' "
                    cm = New MySqlCommand()
                    cm.CommandText = sql
                    cm.CommandType = CommandType.Text
                    cm.Connection = cn
                    cm.ExecuteNonQuery()
                    cn.Close()
                    MessageBox.Show("Administrador eliminado satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

                    'Se actualiza el combobox que muestra los Administradores
                    ComboBox1.Items.Clear()
                    QObtieneAdministradores()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    cn.Close()
                End Try

            End If
        Else
            MessageBox.Show("El Administrador tiene Alumnos asociados, primero elimine aquellos alumnos en GESTION ALUMNO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'Al presionar el BOTON ELIMINAR
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ComboBox1.SelectedItem <> Nothing Then
            'Se realiza un DELETE de un apoderado
            QEliminaAdministrador()

            ingresar = False
            modificar = False

            MaskedTextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            ComboBox1.Text = ""

        Else
            MessageBox.Show("Seleccione un Administrador", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    'Procedimiento que realiza Query para Ingresar un Administrador
    Private Sub QIngresaAdministrador()
        If SoloApoderado() = True Then
            Try
                cn.Open()
                sql = " UPDATE usuario SET tipo_usuario_id='3' WHERE rut='" & MaskedTextBox1.Text & "' "
                cm = New MySqlCommand()
                cm.CommandText = sql
                cm.CommandType = CommandType.Text
                cm.Connection = cn
                cm.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Apoderado ingresado como Administrador Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)

                'Se actualiza el combobox que muestra los apoderados
                ComboBox1.Items.Clear()
                QObtieneAdministradores()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                cn.Close()
            End Try
        Else
            Try
                cn.Open()
                sql = " INSERT INTO usuario (rut,nombre, contrasena, tipo_usuario_id) VALUES ('" & MaskedTextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','1')"
                cm = New MySqlCommand()
                cm.CommandText = sql
                cm.CommandType = CommandType.Text
                cm.Connection = cn
                cm.ExecuteNonQuery()
                cn.Close()
                MessageBox.Show("Administrador Ingresado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
            Catch ex As Exception
                MessageBox.Show("EL Administrador ya existe, ingrese otro Administrador", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                cn.Close()
            End Try
        End If
    End Sub

    'Procedimiento que realiza Query para Modificar un Administrador
    Private Sub QModificaAdministrador()

        Try
            cn.Open()
            sql = " UPDATE usuario SET rut='" & MaskedTextBox1.Text & "', nombre='" & TextBox2.Text & "', contrasena='" & TextBox3.Text & "' WHERE rut='" & ComboBox1.SelectedItem.ToString & "' "
            cm = New MySqlCommand()
            cm.CommandText = sql
            cm.CommandType = CommandType.Text
            cm.Connection = cn
            cm.ExecuteNonQuery()
            cn.Close()
            MessageBox.Show("Administrador Modificado Satisfactoriamente", "Correcto!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            cn.Close()
        End Try
    End Sub

    'Funcion que comprueba si los campos Texbox están vacios
    Private Function CamposLlenos() As Boolean
        Dim Llenos As Boolean = True
        If MaskedTextBox1.Text = "" Then
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
                    QIngresaAdministrador()
                End If

                'Se realiza un UPDATE de una asignatura
                If modificar = True Then
                    QModificaAdministrador()
                End If

                ingresar = False
                modificar = False

                MaskedTextBox1.Clear()
                TextBox2.Clear()
                TextBox3.Clear()
                'Se actualiza el listbox que muestra las asignaturas
                ComboBox1.Text = ""
                ComboBox1.Items.Clear()

                QObtieneAdministradores()

            End If
        Else
            MessageBox.Show("Ingrese todos los datos del Administrador", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub
End Class