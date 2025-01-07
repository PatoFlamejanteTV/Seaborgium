Imports Microsoft.Web.WebView2.WinForms
Imports Microsoft.Web.WebView2.Core
Imports System.CodeDom.Compiler
Imports Microsoft.VisualBasic
Imports IronPython.Hosting
Imports HtmlAgilityPack
Imports System.Text.RegularExpressions
Imports LuaInterface

Public Class Form1
    
    Public luascripting As New Lua()
    Private WithEvents WebView21 As New WebView2()

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the WebView2 control
        WebView21.Dock = DockStyle.Fill
        Me.Controls.Add(WebView21)
        Await WebView21.EnsureCoreWebView2Async(Nothing)

        WebView21.Source = New Uri("https://ultimatequack.neocities.org/mybrowsertest")
    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        ProcessInput(txtAddressBar.Text)
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        If WebView21.CanGoBack Then
            WebView21.GoBack()
        End If
    End Sub

    Private Sub btnForward_Click(sender As Object, e As EventArgs) Handles btnForward.Click
        If WebView21.CanGoForward Then
            WebView21.GoForward()
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        WebView21.Reload()
    End Sub

    ' This event is triggered when the WebView2 initialization is completed
    Private Sub WebView21_CoreWebView2InitializationCompleted(sender As Object, e As CoreWebView2InitializationCompletedEventArgs) Handles WebView21.CoreWebView2InitializationCompleted
        ' Update the address bar with the current URL
        'txtAddressBar.Text = WebView21.Source.ToString()
        ' Update the title label with the current title
        'Me.Text = "Title: " & WebView21.CoreWebView2.DocumentTitle
    End Sub

    ' This event is triggered when navigation is completed
    Private Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs) Handles WebView21.NavigationCompleted
        ' Update the address bar with the current URL
        txtAddressBar.Text = WebView21.Source.ToString()
        ' Update the title label with the current title
        Me.Text = "Title: " & WebView21.CoreWebView2.DocumentTitle

        Dim currentDirectory As String = Environment.CurrentDirectory
        ' Define a JavaScript variable to store the directory
        Dim setDirScript As String = $"internal.seaborgium.currentDir = '{currentDirectory}';"
        WebView21.CoreWebView2.ExecuteScriptAsync(setDirScript)

        Console.WriteLine("EXECUTED:" & setDirScript)

        ' Update the size label with the size of the content (if possible)
        Dim sizeScript As String = "document.documentElement.outerHTML.length.toString()"
        WebView21.CoreWebView2.ExecuteScriptAsync(sizeScript).ContinueWith(Sub(task)
                                                                               Dim result As String = task.Result
                                                                               Invoke(New Action(Sub()
                                                                                                     SiteSize.Text = "Size: " & result & " bytes"
                                                                                                 End Sub))
                                                                           End Sub)




        ' Process the page content for <python> and <vbnet> tags
        ProcessPageContent()
    End Sub

    ' Process the input URL or code
    Private Sub ProcessInput(input As String)
        If input.StartsWith("javascript:") Then
            'JavaScript code
            Dim jsCode As String = input.Substring("javascript:".Length)
            WebView21.CoreWebView2.ExecuteScriptAsync(jsCode)
        ElseIf input.StartsWith("vbnet:") Then
            'VB.NET code
            Dim vbCode As String = input.Substring("vbnet:".Length)
            ExecuteVbNetCode(vbCode)
        ElseIf input.StartsWith("python:") Then
            'Python code
            Dim pythonCode As String = input.Substring("python:".Length)
            ExecutePythonCode(pythonCode)
        ElseIf input.StartsWith("lua:") Then
            'VB.NET code
            Dim luaCode As String = input.Substring("lua:".Length)
            ExecuteLuaCode(luaCode)
        Else
            ' Add protocol if missing
            If Not input.StartsWith("https://") AndAlso Not input.StartsWith("http://") Then
                input = "https://" & input
            End If
            ' Navigate to the URL
            WebView21.Source = New Uri(input)
        End If
    End Sub

    ' Method to execute VB.NET code
    Private Sub ExecuteVbNetCode(vbCode As String)
        Try
            Dim vbProvider As New VBCodeProvider()
            Dim parameters As New CompilerParameters()
            parameters.GenerateInMemory = True

            ' Add necessary assemblies
            parameters.ReferencedAssemblies.Add("System.dll")
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll")

            ' Wrap the code in a simple class and method
            Dim wrappedCode As String = "
                Imports System
                Imports System.Windows.Forms
                Public Class Script
                    Public Sub Execute()
                        " & vbCode & "
                    End Sub
                End Class
            "

            ' Compile the code
            Dim results As CompilerResults = vbProvider.CompileAssemblyFromSource(parameters, wrappedCode)

            If results.Errors.HasErrors Then
                MessageBox.Show("Error in VB.NET code: " & results.Errors(0).ErrorText)
            Else
                ' Execute the compiled code
                Dim assembly As Reflection.Assembly = results.CompiledAssembly
                Dim scriptType As Type = assembly.GetType("Script")
                Dim scriptInstance As Object = Activator.CreateInstance(scriptType)
                scriptType.GetMethod("Execute").Invoke(scriptInstance, Nothing)
            End If
        Catch ex As Exception
            MessageBox.Show("Exception: " & ex.Message)
        End Try
    End Sub

    ' Method to execute Python code
    Private Sub ExecutePythonCode(pythonCode As String)
        Try
            ' Create a new Python engine
            Dim engine = Python.CreateEngine()
            ' Execute the Python code
            engine.Execute(pythonCode)
        Catch ex As Exception
            MessageBox.Show("Error executing Python code: " & ex.Message)
        End Try
    End Sub

    ' Method to execute Lua code
    Private Sub ExecuteLuaCode(LuaCode As String)
        Try
            luascripting.DoString(LuaCode)
        Catch ex As Exception
            MessageBox.Show("Error executing Lua code: " & ex.Message)
        End Try
    End Sub

    ' Custom method to decode HTML entities manually
    Private Function DecodeHtmlEntities(html As String) As String
        ' Replace common HTML entities
        html = html.Replace("\u003C", "<")
        html = html.Replace("\u003E", ">")
        html = html.Replace("\u0026", "&")
        'html = html.Replace("\u0022", Chr(34))
        html = html.Replace("\u0027", "'")
        html = html.Replace("\n", Environment.NewLine)
        ' Add more replacements if needed

        ' Use regex to replace numeric character references
        html = Regex.Replace(html, "\\u([0-9A-Fa-f]{4})", Function(m) ChrW(Convert.ToInt32(m.Groups(1).Value, 16)).ToString())

        Return html
    End Function

    ' Process the page content for <python> and <vbnet> tags
    Private Sub ProcessPageContent()
        Try
            ' Get the HTML content of the current page
            WebView21.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML").ContinueWith(Sub(task)
                                                                                                             Dim htmlContent As String = task.Result
                                                                                                             Invoke(New Action(Sub()
                                                                                                                                   ' Decode the HTML content manually
                                                                                                                                   Dim decodedHtmlContent As String = DecodeHtmlEntities(htmlContent)
                                                                                                                                   ' Debug: Print the HTML content
                                                                                                                                   Console.WriteLine("HTML Content: " & decodedHtmlContent)

                                                                                                                                   ' Load the HTML content into HtmlAgilityPack
                                                                                                                                   Dim htmlDoc As New HtmlDocument()
                                                                                                                                   htmlDoc.LoadHtml(decodedHtmlContent)

                                                                                                                                   ' Process <python> tags
                                                                                                                                   Dim pythonNodes = htmlDoc.DocumentNode.SelectNodes("//python")
                                                                                                                                   If pythonNodes IsNot Nothing Then
                                                                                                                                       Console.WriteLine("Detected <python> nodes!")
                                                                                                                                       For Each pythonNode In pythonNodes
                                                                                                                                           Dim pythonCode As String = pythonNode.InnerText.Replace("\n", Environment.NewLine).Replace("\\", "\")
                                                                                                                                           MessageBox.Show("Python: " & pythonCode)
                                                                                                                                           ExecutePythonCode(pythonCode)
                                                                                                                                           Console.WriteLine("Executed <python> node with the following code: " & pythonCode)
                                                                                                                                       Next
                                                                                                                                   Else
                                                                                                                                       Console.WriteLine("No <python> nodes detected.")
                                                                                                                                   End If

                                                                                                                                   ' Process <vbnet> tags
                                                                                                                                   Dim vbNetNodes = htmlDoc.DocumentNode.SelectNodes("//vbnet")
                                                                                                                                   If vbNetNodes IsNot Nothing Then
                                                                                                                                       Console.WriteLine("Detected <vbnet> nodes!")
                                                                                                                                       For Each vbNetNode In vbNetNodes
                                                                                                                                           Dim vbNetCode As String = vbNetNode.InnerText.Replace("\n", Environment.NewLine).Replace("\\", "\")
                                                                                                                                           ' Ensure VB.NET strings use double quotes
                                                                                                                                           vbNetCode = vbNetCode.Replace("'", """")
                                                                                                                                           MessageBox.Show("VB.NET: " & vbNetCode)
                                                                                                                                           ExecuteVbNetCode(vbNetCode)
                                                                                                                                           Console.WriteLine("Executed <vbnet> node with the following code: " & vbNetCode)
                                                                                                                                       Next
                                                                                                                                   Else
                                                                                                                                       Console.WriteLine("No <vbnet> nodes detected.")
                                                                                                                                   End If

                                                                                                                                   ' Process <lua> tags
                                                                                                                                   Dim luaNodes = htmlDoc.DocumentNode.SelectNodes("//lua")
                                                                                                                                   If luaNodes IsNot Nothing Then
                                                                                                                                       Console.WriteLine("Detected <lua> nodes!")
                                                                                                                                       For Each luaNode In luaNodes
                                                                                                                                           Dim luaCode As String = luaNode.InnerText.Replace("\n", Environment.NewLine).Replace("\\", "\")
                                                                                                                                           ' Ensure VB.NET strings use double quotes
                                                                                                                                           luaCode = luaCode.Replace("'", """")
                                                                                                                                           MessageBox.Show("Lua: " & luaCode)
                                                                                                                                           ExecuteVbNetCode(luaCode)
                                                                                                                                           Console.WriteLine("Executed <lua> node with the following code: " & luaCode)
                                                                                                                                       Next
                                                                                                                                   Else
                                                                                                                                       Console.WriteLine("No <lua> nodes detected.")
                                                                                                                                   End If
                                                                                                                               End Sub))
                                                                                                         End Sub)
        Catch ex As Exception
            MessageBox.Show("Error processing page content: " & ex.Message)
        End Try
    End Sub
End Class