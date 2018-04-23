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
	Public Class SelectionOperation
		Inherits TreeListOperation
		Private block As Block
		Private result_Renamed As String = String.Empty
		Private Const CellDelimeter As String = Constants.vbTab
		Private Const LineDelimeter As String = Constants.vbCrLf

		Public Sub New(ByVal block As Block)
			Me.block = block
		End Sub
		Protected Sub New()

		End Sub


		Public ReadOnly Property Result() As String
			Get
				Return result_Renamed
			End Get
		End Property

		Public Overrides Sub Execute(ByVal node As TreeListNode)
			If (Not block.Between(block.Y1, block.Y2, BlockSelectionHelper.GetNodeIndex(node))) Then
				Return
			End If
			For Each column As TreeListColumn In node.TreeList.Columns
				If block.Contains(column.VisibleIndex, node.TreeList.GetVisibleIndexByNode(node)) Then
					result_Renamed &= node.GetDisplayText(column)
					result_Renamed &= CellDelimeter
				End If
			Next column
			result_Renamed &= LineDelimeter
		End Sub
	End Class
End Namespace
