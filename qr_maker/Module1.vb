Imports ZXing
Imports System.Drawing

Module Module1
    Dim value As String
    Dim file As String
    Dim NonBlock As Boolean = True

    Sub Main()
        Try
            ExecuteParams()
            'text in QR-Code Umwandeln und speichern
            Dim bw As New ZXing.BarcodeWriter
            bw.Format = BarcodeFormat.QR_CODE
            bw.Options.Height = 1
            bw.Options.Width = 1
            bw.Options.Margin = 0
            Dim bmp As Bitmap = bw.Write(value)
            If file.ToLower.EndsWith(".bmp") Then
                'Save as Bitmap
                bmp.Save(file)
            ElseIf file.ToLower.EndsWith(".txt") Then
                'Save as Textfile
                Dim qrtxt As String = ""
                For y = 0 To bmp.Height - 2 Step 2
                    For x As Integer = 0 To bmp.Width - 1
                        If bmp.GetPixel(x, y) = Color.FromArgb(0, 0, 0) And bmp.GetPixel(x, y + 1) = Color.FromArgb(0, 0, 0) Then
                            qrtxt &= IIf(NonBlock, "X", "█")
                        ElseIf bmp.GetPixel(x, y) = Color.FromArgb(0, 0, 0) And bmp.GetPixel(x, y + 1) <> Color.FromArgb(0, 0, 0) Then
                            qrtxt &= IIf(NonBlock, "O", "▀")
                        ElseIf bmp.GetPixel(x, y) <> Color.FromArgb(0, 0, 0) And bmp.GetPixel(x, y + 1) = Color.FromArgb(0, 0, 0) Then
                            qrtxt &= IIf(NonBlock, "U", "▄")
                        Else
                            qrtxt &= " "
                        End If
                    Next
                    qrtxt &= IIf(NonBlock, "E", vbCrLf)
                Next
                'Last row
                Dim ly = bmp.Height - 1
                For x As Integer = 0 To bmp.Width - 1
                    If bmp.GetPixel(x, ly) = Color.FromArgb(0, 0, 0) Then
                        qrtxt &= IIf(NonBlock, "O", "▀")
                    ElseIf bmp.GetPixel(x, ly) <> Color.FromArgb(0, 0, 0) Then
                        qrtxt &= " "
                    End If
                Next
                System.IO.File.WriteAllText(file, qrtxt)
            Else
                ShowHelp()
            End If
        Catch ex As Exception
            ShowHelp()
            Console.WriteLine(vbCrLf & "Error: " & ex.Message)
            End
        End Try
        Console.WriteLine(vbCrLf & "QR-Code succsessfully generated :-)")
        End
    End Sub

    Public Sub ExecuteParams()
        Dim args As String()
        args = Environment.GetCommandLineArgs()

        If args.Length < 3 Or args.Length > 4 Then

            ShowHelp()
            End
        End If

        If Not (args(2).ToLower.EndsWith(".bmp") Or args(2).ToLower.EndsWith(".txt")) Then
            Console.WriteLine(args(2).ToLower)
            ShowHelp()
            End
        End If

        value = args(1)
        file = args(2)

        If args.Length = 4 Then
            If args(3).ToLower = "-symbol" Then
                NonBlock = False
            End If
        End If

    End Sub

    Private Sub ShowHelp()
        Console.WriteLine("Commandline-Tool for generating QR-Codes with ZXing.NET.DLL (https: //zxingnet.codeplex.com/)" & vbCrLf &
                          "Two Parameters needed:" & vbCrLf &
                          "1. Value to Encode" & vbCrLf &
                          "2. Path&File to save (BMP or TXT)" & vbCrLf &
                          "3. -symbol (Optional, only needed for TXT-File)" & vbCrLf & vbCrLf &
                          "If third Parameter is not given, the result Textfile will contain Non-Block-Symbols:" & vbCrLf &
                          "█ = X, ▀ = O, ▄ = U, ' ' = ' ', E = CrLf" & vbCrLf & vbCrLf &
                          "Samples:" & vbCrLf &
                          "qr_maker ""TESTValue12345"" ""c:\reslutQR.bmp"" " & vbCrLf &
                          "qr_maker ""This is a Block-Symbol-QR"" ""c:\tmp\result_block_qr.txt"" -symbol" & vbCrLf &
                          "qr_maker ""This is a Text-NonBlock-Symbol-QR"" ""c\tmp\result_noblock_qr.txt"" ")
    End Sub

End Module
