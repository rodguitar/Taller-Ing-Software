Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class MenuPrincipal
    Dim conex As New MySqlConnection("data source=tallerdb2014.db.8912402.hostedresource.com; user id=tallerdb2014; password=S1emens@; database=tallerdb2014")
    Dim da As MySqlDataAdapter
    Dim dt As DataTable
    Dim ConsultaSql As String
    Dim comando As MySqlCommand
    Dim DR As DataRow
    Dim dset As New System.Data.DataSet


    Private Sub ButtonIngresar_Click(sender As Object, e As EventArgs) Handles ButtonIngresar.Click
        If rutAdmin.Text = "" Then
            MessageBox.Show("Ingrese Administrador", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            rutAdmin.Focus()
        ElseIf claveAdmin.Text = "" Then
            MessageBox.Show("Ingrese Contraseña", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            claveAdmin.Focus()
        Else
            If QValidaUsuario(rutAdmin.Text.Trim, claveAdmin.Text.Trim, "1") Then
                MessageBox.Show("Bienvenido, has ingresado al Sistema", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                GroupBox2.Enabled = False

                'se limpian los campos de acceso
                rutAdmin.Clear()
                claveAdmin.Clear()

                'Se rellenan los datos del adminstrador en la pantalla
                Dim usuarios As DataSet = New DataSet
                da.Fill(usuarios, "Usuarios")

                LabelRUT.Text = usuarios.Tables(0).Rows(0)(0).ToString
                LabelNOMBRE.Text = usuarios.Tables(0).Rows(0)(1).ToString

                'Se habilitan los botones de gestiones
                Button1.Enabled = True
                Button2.Enabled = True
                Button3.Enabled = True
                Button4.Enabled = True
                Button5.Enabled = True
                Button6.Enabled = True
                Button7.Enabled = True
                Button8.Enabled = True
            Else
                If QValidaUsuario(rutAdmin.Text.Trim, claveAdmin.Text.Trim, "3") Then
                    MessageBox.Show("Bienvenido, has ingresado al Sistema", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    GroupBox2.Enabled = False

                    'se limpian los campos de acceso
                    rutAdmin.Clear()
                    claveAdmin.Clear()

                    'Se rellenan los datos del adminstrador en la pantalla
                    Dim usuarios As DataSet = New DataSet
                    da.Fill(usuarios, "Usuarios")

                    LabelRUT.Text = usuarios.Tables(0).Rows(0)(0).ToString
                    LabelNOMBRE.Text = usuarios.Tables(0).Rows(0)(1).ToString


                    'Se habilitan los botones de gestiones
                    Button1.Enabled = True
                    Button2.Enabled = True
                    Button3.Enabled = True
                    Button4.Enabled = True
                    Button5.Enabled = True
                    Button6.Enabled = True
                    Button7.Enabled = True
                    Button8.Enabled = True
                Else
                    MessageBox.Show("Usuario y/o contraseña Incorrectos", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                End If
            End If
        End If



    End Sub

    'funcion que realiza la consulta SQL a la base de datos y  valida el usuario
    Private Function QValidaUsuario(ByVal usuario As String, ByVal clave As String, ByVal tipo_usuario As String) As Boolean
        Try
            ConsultaSql = "SELECT rut,Nombre,tipo_usuario_id from usuario as sesion WHERE rut=@usu and contrasena=@clav and tipo_usuario_id=@tipo limit 1"
            da = New MySqlDataAdapter(ConsultaSql, conex)
            da.SelectCommand.Parameters.AddWithValue("@usu", usuario)
            da.SelectCommand.Parameters.AddWithValue("@clav", clave)
            da.SelectCommand.Parameters.AddWithValue("@tipo", tipo_usuario)

            da.SelectCommand.CommandType = CommandType.Text
            dt = New DataTable
            da.Fill(dt)


            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As MySqlException
            MessageBox.Show(ex.Message, usuario)
        End Try
        Return True
    End Function

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click

        'Se habilita los campos sesion
        GroupBox2.Enabled = True
        'Se deshabilitan los botones de gestiones
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        Button5.Enabled = False
        Button6.Enabled = False
        Button7.Enabled = False
        Button8.Enabled = False
        'se limpian los campos de datos de usuario de la pantalla principal
        LabelRUT.ResetText()
        LabelNOMBRE.ResetText()

        'Cierre de todos los Forms( ventanas de gestiones abiertas)
        GestionCurso.Close()
        GestionAlumno.Close()
        GestionAnotacion.Close()
        GestionAsignatura.Close()
        GestionComunicacion.Close()
        GestionEvaluacion.Close()
        GestionApoderado.Close()



    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        GestionCurso.Show()
    End Sub

    'BORRAR ESTE METODO DESPUES AL FINALIZAR LAS PRUEBAS!!!!!!!!!!!!!!!!!!!!!!!!!!
    Private Sub MenuPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'EJECUTCIONES DE PRUEBA QUE HAY QUE BORRAR
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
        Button5.Enabled = True
        Button6.Enabled = True
        Button7.Enabled = True
        Button8.Enabled = True
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        GestionAsignatura.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GestionApoderado.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        GestionAnotacion.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GestionAlumno.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        GestionComunicacion.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        GestionEvaluacion.Show()
    End Sub
End Class
