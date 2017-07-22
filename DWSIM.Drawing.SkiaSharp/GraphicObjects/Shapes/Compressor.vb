﻿Imports DWSIM.Drawing.SkiaSharp.GraphicObjects
Imports DWSIM.Interfaces.Enums.GraphicObjects

Namespace GraphicObjects.Shapes

    Public Class CompressorGraphic

        Inherits ShapeGraphic

#Region "Constructors"

        Public Sub New()
            Me.ObjectType = DWSIM.Interfaces.Enums.GraphicObjects.ObjectType.Compressor
            Me.Description = "Adiabatic Compressor"
        End Sub

        Public Sub New(ByVal graphicPosition As SKPoint)
            Me.New()
            Me.SetPosition(graphicPosition)
        End Sub

        Public Sub New(ByVal posX As Integer, ByVal posY As Integer)
            Me.New(New SKPoint(posX, posY))
        End Sub

        Public Sub New(ByVal graphicPosition As SKPoint, ByVal graphicSize As SKSize)
            Me.New(graphicPosition)
            Me.SetSize(graphicSize)
        End Sub

        Public Sub New(ByVal posX As Integer, ByVal posY As Integer, ByVal graphicSize As SKSize)
            Me.New(New SKPoint(posX, posY), graphicSize)
        End Sub

        Public Sub New(ByVal posX As Integer, ByVal posY As Integer, ByVal width As Integer, ByVal height As Integer)
            Me.New(New SKPoint(posX, posY), New SKSize(width, height))
        End Sub

#End Region

        Public Overrides Sub CreateConnectors(InCount As Integer, OutCount As Integer)

            Dim myIC1 As New ConnectionPoint
            myIC1.Position = New Point(X, Y + 0.5 * Height)
            myIC1.Type = ConType.ConIn

            Dim myIC2 As New ConnectionPoint
            myIC2.Position = New Point(X + 0.5 * Width, Y + Height)
            myIC2.Type = ConType.ConEn

            Dim myOC1 As New ConnectionPoint
            myOC1.Position = New Point(X + Width, Y + 0.5 * Height)
            myOC1.Type = ConType.ConOut

            Me.EnergyConnector.Position = New Point(X + 0.5 * Width, Y + Height)
            Me.EnergyConnector.Type = ConType.ConEn
            Me.EnergyConnector.Direction = ConDir.Up
            Me.EnergyConnector.Active = False

            With InputConnectors

                If .Count = 2 Then
                    If Me.FlippedH Then
                        .Item(0).Position = New Point(X + Width, Y + 0.5 * Height)
                        .Item(1).Position = New Point(X + 0.5 * Width, Y + Height)
                    Else
                        .Item(0).Position = New Point(X, Y + 0.5 * Height)
                        .Item(1).Position = New Point(X + 0.5 * Width, Y + Height)
                    End If
                Else
                    .Add(myIC1)
                    .Add(myIC2)
                End If

                .Item(0).ConnectorName = "Inlet"
                .Item(1).ConnectorName = "Energy Stream"

            End With

            With OutputConnectors

                If .Count <> 0 Then
                    If Me.FlippedH Then
                        .Item(0).Position = New Point(X, Y + 0.5 * Height)
                    Else
                        .Item(0).Position = New Point(X + Width, Y + 0.5 * Height)
                    End If
                Else
                    .Add(myOC1)
                End If

                .Item(0).ConnectorName = "Outlet"

            End With

        End Sub

        Public Overrides Sub Draw(ByVal g As Object)

            Dim canvas As SKCanvas = DirectCast(g, SKCanvas)

            CreateConnectors(0, 0)
            UpdateStatus()

            MyBase.Draw(g)


            Dim myPen As New SKPaint()
            With myPen
                .Color = LineColor
                .StrokeWidth = LineWidth
                .IsStroke = Not Fill
                .IsAntialias = GlobalSettings.Settings.DrawingAntiAlias
            End With

            Dim rect As New SKRect(X, Y, X + Width, X + Height)

            Dim gp As New SKPath()
            If Me.FlippedH = False Then
                gp.MoveTo(Convert.ToInt32(X), Convert.ToInt32(Y))
                gp.LineTo(Convert.ToInt32(X + Width), Convert.ToInt32(Y + 0.3 * Height))
                gp.LineTo(Convert.ToInt32(X + Width), Convert.ToInt32(Y + 0.7 * Height))
                gp.LineTo(Convert.ToInt32(X), Convert.ToInt32(Y + Height))
                gp.LineTo(Convert.ToInt32(X), Convert.ToInt32(Y))
                gp.Close()
            Else
                gp.MoveTo(Convert.ToInt32(X + Width), Convert.ToInt32(Y))
                gp.LineTo(Convert.ToInt32(X + Width), Convert.ToInt32(Y))
                gp.LineTo(Convert.ToInt32(X), Convert.ToInt32(Y + 0.3 * Height))
                gp.LineTo(Convert.ToInt32(X), Convert.ToInt32(Y + 0.7 * Height))
                gp.LineTo(Convert.ToInt32(X + Width), Convert.ToInt32(Y + Height))
                gp.LineTo(Convert.ToInt32(X + Width), Convert.ToInt32(Y))
                gp.Close()
            End If

            canvas.DrawPath(gp, myPen)

        End Sub

    End Class

End Namespace