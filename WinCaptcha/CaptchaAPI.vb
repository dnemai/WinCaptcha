Imports System.IO
Imports System.Net
Imports System.Threading

Public Class CaptchaAPI

    Public Property APIKey() As String
        Get
            Return m_APIKey
        End Get
        Private Set
            m_APIKey = Value
        End Set
    End Property
    Private m_APIKey As String
    Public Sub New(p_apiKey As String)
        APIKey = p_apiKey
    End Sub

    Public Function SolveCaptcha(googleKey As String, pageUrl As String, proxy As String,
                                     proxyType__1 As ProxyType, ByRef result As String) As Boolean
        Dim requestUrl As String = (Convert.ToString((Convert.ToString((Convert.ToString((Convert.ToString("http://2captcha.com/in.php?key=") & APIKey) + "&method=userrecaptcha&googlekey=") & googleKey) + "&pageurl=") & pageUrl) + "&proxy=") & proxy) + "&proxytype="

        Select Case proxyType__1
            Case ProxyType.HTTP
                requestUrl += "HTTP"
                Exit Select
            Case ProxyType.HTTPS
                requestUrl += "HTTPS"
                Exit Select
            Case ProxyType.SOCKS4
                requestUrl += "SOCKS4"
                Exit Select
            Case ProxyType.SOCKS5
                requestUrl += "SOCKS5"
                Exit Select
        End Select

        Try
            Dim req As WebRequest = WebRequest.Create(requestUrl)

            Using resp As WebResponse = req.GetResponse()
                Using read As New StreamReader(resp.GetResponseStream())
                    Dim response As String = read.ReadToEnd()

                    If response.Length < 3 Then
                        result = response
                        Return False
                    Else
                        If response.Substring(0, 3) = "OK|" Then
                            Dim captchaID As String = response.Remove(0, 3)

                            For i As Integer = 0 To 23
                                Dim getAnswer As WebRequest = WebRequest.Create(Convert.ToString((Convert.ToString("http://2captcha.com/res.php?key=") & APIKey) + "&action=get&id=") & captchaID)

                                Using answerResp As WebResponse = getAnswer.GetResponse()
                                    Using answerStream As New StreamReader(answerResp.GetResponseStream())
                                        Dim answerResponse As String = answerStream.ReadToEnd()

                                        If answerResponse.Length < 3 Then
                                            result = answerResponse
                                            Return False
                                        Else
                                            If answerResponse.Substring(0, 3) = "OK|" Then
                                                result = answerResponse.Remove(0, 3)
                                                Return True
                                            ElseIf answerResponse <> "CAPCHA_NOT_READY" Then
                                                result = answerResponse
                                                Return False
                                            End If
                                        End If
                                    End Using
                                End Using

                                Thread.Sleep(5000)
                            Next

                            result = "Timeout"
                            Return False
                        Else
                            result = response
                            Return False
                        End If
                    End If
                End Using
            End Using
        Catch
        End Try

        result = "Unknown error"
        Return False
    End Function


    Public Enum ProxyType
        HTTP
        HTTPS
        SOCKS4
        SOCKS5
    End Enum



End Class
