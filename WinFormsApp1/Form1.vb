
Option Explicit On
Option Strict On

Public Class Form1
    ''//NOTE: The circles in this code are bound to a rectangle but it should be fairly trivial to create a master circle and check that

    ''//Dimension of the bounding image
    Private Shared ReadOnly ImageMaxDimension As Integer = 500
    Private Shared ReadOnly MinCircleDiameter As Integer = 4
    Private Shared ReadOnly MaxCircleDiameter As Integer = 15
    Private Shared ReadOnly CircleCount As Integer = 5000

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''//Create a picture box to output to
        Dim PB As New PictureBox()
        PB.Dock = DockStyle.Fill
        Me.Controls.Add(PB)

        ''//List of bounds of all circles created so far
        Dim AllBounds As New List(Of RectangleF)

        ''//Our random number generator
        Dim R As New Random()

        ''//Values for our individual circles
        Dim W, X, Y As Integer
        Dim Re As RectangleF

        ''//Create a bitmap to draw on
        Dim TempB As New Bitmap(ImageMaxDimension, ImageMaxDimension)
        Using G = Graphics.FromImage(TempB)

            Dim NumCircle = 0

            For I = 1 To CircleCount
                ''//We can only draw so many circles, this just gives us a counter so we know when we reach the limit for a given size
                Trace.WriteLine(I)
                Dim GiveUp = 100

                ''//Create an infinite loop that we will break out of if we have found a circle that does not intersect anything
                Do While GiveUp < 100
                    GiveUp = GiveUp + 1
                    ''//Create a random diameter
                    W = R.Next(MinCircleDiameter, MaxCircleDiameter + 1)
                    ''//Create a random X,Y
                    X = R.Next(0 + W, ImageMaxDimension - W)
                    Y = R.Next(0 + W, ImageMaxDimension - W)
                    ''//Create our rectangle
                    Re = New RectangleF(X, Y, W, W)

                    ''//Check each existing bound to see if they intersect with the current rectangle
                    For Each B In AllBounds
                        ''//If they do, start the loop over again
                        If DoCirclesIntersect(B, Re, 1) Then Continue Do
                    Next

                    ''//If we are here, no circles intersected, break from the infinite loop
                    Exit Do
                Loop

                If (GiveUp < 100) Then
                    AllBounds.Add(Re)
                    NumCircle = NumCircle + 1
                    TextBox1.Text = "Drawing Circle :" & NumCircle
                End If

                ''/Draw the circle on the screen
                G.FillEllipse(Brushes.BurlyWood, Re)
            Next

            ''//Draw the image to the picture box
            PB.Image = TempB
        End Using
    End Sub

    Private Shared Function DoCirclesIntersect(r1 As RectangleF, r2 As RectangleF,
                                               Optional ByVal PadCircle As Integer = 0) As Boolean
        ''//This code is hopefully what @Sjoerd said in his post
        Dim aX = Math.Pow(r1.X - r2.X, 2)
        Dim aY = Math.Pow(r1.Y - r2.Y, 2)
        Dim Dif = Math.Abs(aX - aY)
        Dim ra1 = r1.Width/2
        Dim ra2 = r2.Width/2
        Dim raDif = Math.Pow(ra1 + ra2, 2)
        Return (raDif + PadCircle) > Dif
    End Function


End Class

