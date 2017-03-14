Imports ZXing
Imports System.Drawing

Module Module1
    Dim value As String
    Dim file As String

    Sub Main()
        Try
            ExecuteParams()
            'text in QR-Code Umwandeln und speichern
            Dim bw As New ZXing.BarcodeWriter
            bw.Format = BarcodeFormat.QR_CODE
            bw.Options.Height = 128
            bw.Options.Width = 128
            bw.Options.Margin = 0
            Dim bm As Bitmap = bw.Write(value)
            bm.Save(file)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        End
    End Sub

    Public Sub ExecuteParams()
        Dim args As String()
        args = Environment.GetCommandLineArgs()

        If args.Length <> 3 Then
            Console.WriteLine("2 Parameter angeben zb: ouput file und value" & vbCrLf &
                              "Beispiel: qr_maker 30000-0-0 result.bmp")
            End
        End If

        value = args(1)
        file = args(2)
    End Sub

End Module
