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
	Friend Class BlockSelectionHelper
		Private fList As TreeList
		Private fBlock As Block

		Public Sub New(ByVal list As TreeList)
			If list Is Nothing Then
				Throw New Exception("A valid TreeList instance must be passed to the constructor")
			End If

			fList = list
			fBlock = New Block()
			SubscribeEvents()
		End Sub

		Private Sub SubscribeEvents()
			AddHandler fList.FocusedColumnChanged, AddressOf fList_FocusedColumnChanged
			AddHandler fList.FocusedNodeChanged, AddressOf fList_FocusedNodeChanged
			AddHandler fList.MouseMove, AddressOf fList_MouseMove
			AddHandler fList.NodeCellStyle, AddressOf fList_NodeCellStyle
			AddHandler fList.BeforeExpand, AddressOf fList_BeforeExpand
			AddHandler fList.BeforeCollapse, AddressOf fList_BeforeCollapse
			AddHandler fList.KeyDown, AddressOf fList_KeyDown
			AddHandler fList.PopupMenuShowing, AddressOf fList_PopupMenuShowing
			AddHandler fList.EditorKeyDown, AddressOf fList_EditorKeyDown
		End Sub

		Private Sub fList_EditorKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.C Then
				Dim editor As TextEdit = TryCast(fList.ActiveEditor, TextEdit)
				If editor Is Nothing Then
					Return
				End If
				If editor.SelectionLength > 0 Then
					Return
				End If
				CopyValuesToClipboard()
			End If
		End Sub

		Private Sub fList_PopupMenuShowing(ByVal sender As Object, ByVal e As PopupMenuShowingEventArgs)
			Dim hi As TreeListHitInfo = fList.CalcHitInfo(e.Point)
			If hi.HitInfoType = HitInfoType.Cell Then
				e.Menu.Items.Add(GetCopyMenuItem())
			End If
		End Sub

		Private Function GetCopyMenuItem() As DevExpress.Utils.Menu.DXMenuItem
			Dim item As New DevExpress.Utils.Menu.DXMenuItem()
			item.Caption = "Copy selected values"
			AddHandler item.Click, AddressOf item_Click
			Return item
		End Function

		Private Sub item_Click(ByVal sender As Object, ByVal e As EventArgs)
			CopyValuesToClipboard()
		End Sub
		Private Sub fList_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.C Then
				CopyValuesToClipboard()
			End If

		End Sub

		Private Sub fList_BeforeCollapse(ByVal sender As Object, ByVal e As BeforeCollapseEventArgs)
			fBlock.Clear()
		End Sub

		Private Sub fList_BeforeExpand(ByVal sender As Object, ByVal e As BeforeExpandEventArgs)
			fBlock.Clear()
		End Sub
		Public ReadOnly Property Block() As Block
			Get
				Return fBlock
			End Get
		End Property

		Public ReadOnly Property List() As TreeList
			Get
				Return fList
			End Get
		End Property

		Public Shared Function GetNodeIndex(ByVal node As TreeListNode) As Integer
			Return node.TreeList.GetVisibleIndexByNode(node)
		End Function
		Private Sub fList_NodeCellStyle(ByVal sender As Object, ByVal e As GetCustomNodeCellStyleEventArgs)
			If fBlock.Contains(e.Column.VisibleIndex, GetNodeIndex(e.Node)) Then
				e.Appearance.BackColor = SystemColors.Highlight
			End If
		End Sub

		Private Sub fList_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			If e.Button = MouseButtons.Left Then
				Dim hInfo As TreeListHitInfo = fList.CalcHitInfo(e.Location)
				If hInfo.HitInfoType = HitInfoType.Cell Then
					fBlock.X2 = hInfo.Column.VisibleIndex
					fBlock.Y2 = GetNodeIndex(hInfo.Node)
					If fBlock.Modified AndAlso (Not fBlock.IsEmpty()) Then
						InvalidateBlock()
					End If
				End If
			End If
		End Sub

		Public Sub InvalidateBlock()
			fList.LayoutChanged()
		End Sub

		Private Sub fList_FocusedNodeChanged(ByVal sender As Object, ByVal e As FocusedNodeChangedEventArgs)
			If fList.FocusedColumn IsNot Nothing Then
				UpdateBlock()
			End If
		End Sub

		Private Sub UpdateBlock()
			If Control.ModifierKeys = Keys.Shift Then
				fBlock.X2 = fList.FocusedColumn.VisibleIndex
				fBlock.Y2 = GetNodeIndex(fList.FocusedNode)
			Else
				fBlock.X1 = fList.FocusedColumn.VisibleIndex
				fBlock.Y1 = GetNodeIndex(fList.FocusedNode)
				fBlock.X2 = fBlock.X1
				fBlock.Y2 = fBlock.Y1
			End If
			If fBlock.Modified Then
				InvalidateBlock()
			End If
		End Sub

		Private Sub fList_FocusedColumnChanged(ByVal sender As Object, ByVal e As FocusedColumnChangedEventArgs)
			UpdateBlock()
		End Sub

		Private Sub CopyValuesToClipboard()
			Dim value As String = GetSelectedValues()
			If (Not String.IsNullOrEmpty(value)) Then
				Clipboard.SetText(value)
			End If
		End Sub
		Public Function GetSelectedValues() As String
			Dim result As String = String.Empty
			If (Not fBlock.IsEmpty()) Then
				Dim operation As New SelectionOperation(fBlock)
				fList.NodesIterator.DoOperation(operation)
				result = operation.Result
			End If
			Return result
		End Function

	End Class

End Namespace
