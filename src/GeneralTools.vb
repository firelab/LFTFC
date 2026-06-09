Imports System.Windows
Imports ArcGIS.Desktop.Core
Imports ArcGIS.Desktop.Core.Geoprocessing

Module GeneralTools

    Public Async Function gt_PixelPYT(thetool As String,
                                   myParams As List(Of String),
                                   MU As String) As Task(Of Boolean)

        Dim working = New WorkingWindow()

        Dim helper = New System.Windows.Interop.WindowInteropHelper(working)
        helper.Owner = Process.GetCurrentProcess().MainWindowHandle

        working.Show()
        Await Task.Yield()

        ProApp.Current.MainWindow.IsEnabled = False

        Try
            Dim pixel_result As IGPResult =
                Await Geoprocessing.ExecuteToolAsync(thetool, myParams, Nothing)

            working.Close()
            ProApp.Current.MainWindow.IsEnabled = True

            Return (Not pixel_result.IsFailed)

        Catch ex As Exception
            working.Close()
            ProApp.Current.MainWindow.IsEnabled = True
            MessageBox.Show(ex.ToString(), "pyt data")
            Return False
        End Try

    End Function

End Module
