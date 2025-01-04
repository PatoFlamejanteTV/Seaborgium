<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnForward = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.txtAddressBar = New System.Windows.Forms.TextBox()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.SiteSize = New System.Windows.Forms.Label()
        Me.WebView22 = New Microsoft.Web.WebView2.WinForms.WebView2()
        CType(Me.WebView22, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(12, 12)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(23, 23)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "<"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnForward
        '
        Me.btnForward.Location = New System.Drawing.Point(41, 12)
        Me.btnForward.Name = "btnForward"
        Me.btnForward.Size = New System.Drawing.Size(23, 23)
        Me.btnForward.TabIndex = 2
        Me.btnForward.Text = ">"
        Me.btnForward.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(70, 12)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(59, 23)
        Me.btnRefresh.TabIndex = 3
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'txtAddressBar
        '
        Me.txtAddressBar.Location = New System.Drawing.Point(135, 14)
        Me.txtAddressBar.Name = "txtAddressBar"
        Me.txtAddressBar.Size = New System.Drawing.Size(459, 20)
        Me.txtAddressBar.TabIndex = 4
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(600, 12)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(28, 23)
        Me.btnGo.TabIndex = 5
        Me.btnGo.Text = "Go"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'SiteSize
        '
        Me.SiteSize.AutoSize = True
        Me.SiteSize.Location = New System.Drawing.Point(634, 17)
        Me.SiteSize.Name = "SiteSize"
        Me.SiteSize.Size = New System.Drawing.Size(43, 13)
        Me.SiteSize.TabIndex = 6
        Me.SiteSize.Text = "Label1"
        '
        'WebView22
        '
        Me.WebView22.AllowExternalDrop = True
        Me.WebView22.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.WebView22.CreationProperties = Nothing
        Me.WebView22.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WebView22.Location = New System.Drawing.Point(12, 42)
        Me.WebView22.Name = "WebView22"
        Me.WebView22.Size = New System.Drawing.Size(776, 396)
        Me.WebView22.TabIndex = 7
        Me.WebView22.ZoomFactor = 1.0R
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.SiteSize)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.txtAddressBar)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.btnForward)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.WebView22)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.WebView22, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBack As Button
    Friend WithEvents btnForward As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents txtAddressBar As TextBox
    Friend WithEvents btnGo As Button
    Friend WithEvents SiteSize As Label
    Friend WithEvents WebView22 As Microsoft.Web.WebView2.WinForms.WebView2
End Class
