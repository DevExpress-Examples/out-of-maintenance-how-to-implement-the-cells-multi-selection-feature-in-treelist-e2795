Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraTreeList
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraTreeList.Nodes.Operations
Imports DevExpress.XtraTreeList.Columns
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.XtraEditors

Namespace WindowsApplication1
	Public Class Block
		Private FX1 As Integer
		Private FX2 As Integer
		Private FY1 As Integer
		Private FY2 As Integer
		Private FModified As Boolean

		Public Function Between(ByVal a As Integer, ByVal b As Integer, ByVal c As Integer) As Boolean
			If a > b Then
				Dim temp As Integer = a
				a = b
				b = temp
			End If
			Return (a <= c) AndAlso (c <= b)
		End Function

		Public Function Contains(ByVal x As Integer, ByVal y As Integer) As Boolean
			Return Between(X1, X2, x) AndAlso Between(Y1, Y2, y)
		End Function
		Public Function IsEmpty() As Boolean
			Return (X1 = X2 AndAlso Y1 = Y2)
		End Function

		Public ReadOnly Property Width() As Integer
			Get
				Return Math.Abs(FX2 - FX1)
			End Get
		End Property

		#Region "Coordinates"
		Public Property X1() As Integer
			Get
				Return FX1
			End Get
			Set(ByVal value As Integer)
				If FX1 <> value Then
					FX1 = value
					FModified = True
				End If
			End Set
		End Property
		Public Property X2() As Integer
			Get
				Return FX2
			End Get
			Set(ByVal value As Integer)
				If FX2 <> value Then
					FX2 = value
					FModified = True
				End If
			End Set
		End Property
		Public Property Y1() As Integer
			Get
				Return FY1
			End Get
			Set(ByVal value As Integer)
				If FY1 <> value Then
					FY1 = value
					FModified = True
				End If
			End Set
		End Property
		Public Property Y2() As Integer
			Get
				Return FY2
			End Get
			Set(ByVal value As Integer)
				If FY2 <> value Then
					FY2 = value
					FModified = True
				End If
			End Set
		End Property
		#End Region

		Public ReadOnly Property Modified() As Boolean
			Get
				Return FModified
			End Get
		End Property

		Public Sub Clear()
			Y2 = 0
			Y1 = Y2
			X2 = Y1
			X1 = X2
		End Sub
	End Class
End Namespace
