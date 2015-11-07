Imports System
Imports System.Data
Imports Npgsql

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim bd As New BaseDeDatos

        'Rellenamos el combobox con Objetos de tipo Juego
        For Each fila As DataRow In bd.ListarPaises().Rows
            ComboBox1.Items.Add(New Juego(fila(0), fila(1)))
        Next

        'ASignamos un DataSet con lo juegos al DataSource y mediante ValueMember y DisplayMember obtenemos el valor seleccionado
        ComboBox2.ValueMember = "id_idonline"
        ComboBox2.DisplayMember = "nombre_idonline"
        ComboBox2.DataSource = bd.ListarPaises()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        MsgBox("He seleccionado el Juego con ID:" & CType(ComboBox1.SelectedItem, Juego).id_idonline)

    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        MsgBox("He seleccionado el Juego con ID:" & ComboBox2.SelectedValue)
    End Sub
End Class

''' <summary>
''' Mini Clase Juego
''' </summary>
Public Class Juego
    Public Property id_idonline As UInteger
    Public Property nombre_idonline As String

    Public Sub New(ByVal pId As UInteger, ByVal pNom As String)
        Me.id_idonline = pId
        Me.nombre_idonline = pNom
    End Sub
    Public Overrides Function ToString() As String
        Return nombre_idonline
    End Function
End Class

Public Class BaseDeDatos
    Private ConexionConBD As NpgsqlConnection
    Private Orden As NpgsqlCommand
    Private Lector As NpgsqlDataReader

    Public Function ListarPaises() As DataTable
        Dim Juegos As DataTable
        Juegos = New DataTable
        Dim strConexión As String = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                                  "localhost", "5432", "esports", "antonio", "esports")
        ConexionConBD = New NpgsqlConnection(strConexión)
        ConexionConBD.Open()

        'Crear una consulta
        Dim Consulta As String = "SELECT id_idonline, nombre_idonline FROM IDONLINE ORDER BY 2"
        Orden = New NpgsqlCommand(Consulta, ConexionConBD)
        Lector = Orden.ExecuteReader()
        Juegos.Load(Lector)
        CerrarConexion()
        Return Juegos
    End Function


    Public Sub CerrarConexion()
        ' Cerrar la conexión cuando ya no sea necesaria
        If (Not Lector Is Nothing) Then
            Lector.Close()
        End If
        If (Not ConexionConBD Is Nothing) Then
            ConexionConBD.Close()
        End If
    End Sub

End Class