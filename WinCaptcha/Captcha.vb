Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers

Public Class Captcha
    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        txtResult.Text = UploadMultipartAsync()
    End Sub

    Public Function UploadMultipartAsync() As String
        Try
            'file
            Dim fileContent = New ByteArrayContent(File.ReadAllBytes("../../Random.jpg"))
            ' fileContent.Headers.ContentDisposition = New ContentDispositionHeaderValue("attachment")

            Dim httpClient As New HttpClient()
            Dim form As New MultipartFormDataContent()
            Dim key As String = "37ab89c12409fefcaf82beb8ca5fe136"
            form.Add(New StringContent(key), "key")
            form.Add(fileContent, "file", "Random.jpg")
            Dim response As HttpResponseMessage = httpClient.PostAsync("http://2captcha.com/in.php", form).Result
            response.EnsureSuccessStatusCode()
            httpClient.Dispose()
            Dim sd As String = response.Content.ReadAsStringAsync().Result
            Return sd

        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class
