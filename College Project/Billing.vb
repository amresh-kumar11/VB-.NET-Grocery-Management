Imports System.Data.SqlClient
Imports System.Drawing.Printing
Public Class Billing
    Private Sub AddBill()
        Try
            con.open()
            'Dim query = "insert into BillTbl values('" & ClNameTb.Text & "'," & GrdTotal & ",'" & DateTime.Today.Date & "')" 'error conversion failed when converting date or time from character string
            Dim query = "insert into BillTbl values('" & ClNameTb.Text & "'," & GrdTotal & ",'" & DateTime.Today.ToString("yyyy-MM-dd") & "')"   'error free
            'Dim formattedDate As String = DateTime.Today.ToString("yyyy-MM-dd")
            Dim cmd As SqlCommand
            cmd = New SqlCommand(query, con)
            cmd.ExecuteNonQuery()
            MsgBox("Bill  saved Successfully")
            con.close()
            TotaLbill.Text = "Total"
            Guna2DataGridView1.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub
    Private Sub UpdateItem()
        Dim newQty = Stock - Convert.ToInt32(QtyTb.Text)
        Try
            con.open()
            Dim query = "Update ItemTbl set ItQty=" & newQty & " where ItId=" & key & ""
            Dim cmd As SqlCommand
            cmd = New SqlCommand(query, con)
            cmd.ExecuteNonQuery()
            MsgBox("Item Update Successfully")
            con.close()
            DisplayItem()
        Catch ex As Exception

        End Try

    End Sub
    Dim i = 0, GrdTotal = 0

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If PriceTb.Text = "" Or QtyTb.Text = "" Then
            MsgBox("Enter the Quantity")
        ElseIf ItNameTb.Text = "" Then
            MsgBox("Select the item")
        Else
            Dim rnum As Integer = Guna2DataGridView1.Rows.Add()
            i = i + 1
            Dim total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PriceTb.Text)
            Guna2DataGridView1.Rows.Item(rnum).Cells("column1").Value = i
            Guna2DataGridView1.Rows.Item(rnum).Cells("column2").Value = ItNameTb.Text
            Guna2DataGridView1.Rows.Item(rnum).Cells("column3").Value = PriceTb.Text
            Guna2DataGridView1.Rows.Item(rnum).Cells("column4").Value = QtyTb.Text
            Guna2DataGridView1.Rows.Item(rnum).Cells("column5").Value = total
            GrdTotal = GrdTotal + total
            Dim tot As String
            tot = "Rs :" + Convert.ToString(GrdTotal)
            TotaLbill.Text = tot
            UpdateItem()
            DisplayItem()
            Reset()
        End If
    End Sub
    Dim con = New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\.NET\College Project\College Project\college_project.mdf;Integrated Security=True;Connect Timeout=30")
    Private Sub DisplayItem()
        con.open()
        Dim query = "select * from ItemTbl"
        Dim cmd = New SqlCommand(query, con)
        Dim adapter As SqlDataAdapter
        adapter = New SqlDataAdapter(cmd)
        Dim builder As New SqlCommandBuilder(adapter)
        Dim ds As DataSet
        ds = New DataSet
        adapter.Fill(ds)
        ItemDGV.DataSource = ds.Tables(0)
        con.close()
    End Sub
    Private Sub Billing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisplayItem()

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)
        Application.Exit()

    End Sub
    Dim key = 0, Stock = 0
    Private Sub ItemDGV_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles ItemDGV.CellMouseClick
        Dim row As DataGridViewRow = ItemDGV.Rows(e.RowIndex)
        ItNameTb.Text = row.Cells(1).Value.ToString
        PriceTb.Text = row.Cells(3).Value.ToString

        If ItNameTb.Text = "" Then
            key = 0
        Else
            key = Convert.ToInt32(row.Cells(0).Value.ToString)
            Stock = Convert.ToInt32(row.Cells(2).Value.ToString)
        End If
    End Sub
    Private Sub Reset()
        ItNameTb.Text = ""
        PriceTb.Text = ""
        QtyTb.Text = ""
        key = 0
        Stock = 0

    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Reset()
    End Sub

    'Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
    '  If ClNameTb.Text = "" Then
    '    MsgBox("Enter Client Name")
    '   Else
    '  AddBill()
    '--------------------------------------------------------------------------------------------------------------------------------
    ' PrintDocument1.DefaultPageSettings.PaperSize = New PaperSize("MySize", 250, 600)
    'PrintPreviewDialog1.WindowState = FormWindowState.Maximized
    'PrintPreviewDialog1.PrintPreviewControl.Zoom = 2.5
    ' PrintPreviewDialog1.Document = PrintDocument1
    'PrintPreviewDialog1.ShowDialog()


    ' PrintPreviewDialog1.Document = PrintDocument1
    'If PrintPreviewDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
    'PrintDocument1.Print()

    'End If
    ' PrintPreviewDialog1.ShowDialog()


    '  End If
    'End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim logoimg As Image = My.Resources.ResourceManager.GetObject("logo")
        e.Graphics.DrawImage(logoimg, CInt((e.PageBounds.Width - 450) / 2), 675, 500, 300)

        e.Graphics.DrawString("GROCERY SHOP", New Font("Times New Roman", 50, FontStyle.Bold), Brushes.Black, 140, 35)
        e.Graphics.DrawString("-----------------------------------------------------------------------------------------------", New Font("Arial", 20), Brushes.Black, 0, 100)

        e.Graphics.DrawImage(bitmap, 170, 160)
        Dim Printview As RectangleF = e.PageSettings.PrintableArea
        If Me.Guna2DataGridView1.Height - Printview.Height > 0 Then
            e.HasMorePages = True

        End If
        e.Graphics.DrawString("-----------------------------------------------------------------------------------------------", New Font("Arial", 20), Brushes.Black, 0, 550)
        'Display the total amount on the bill
        e.Graphics.DrawString("Total Amount Rs : " + GrdTotal.ToString, New Font("Times New Roman", 20), Brushes.Black, 305, 535)

        'Display a thank you message
        e.Graphics.DrawString("Thank You ! " + ClNameTb.Text.ToString, New Font("Times New Roman", 25, FontStyle.Bold), Brushes.Black, 220, 575)

        '  e.Graphics.DrawString("Thanks For Buying In Our Shop ", New Font("Times New Roman", 20), Brushes.Black, 220, 575)
        e.Graphics.DrawString("Please Visit Again Our Shop ", New Font("Times New Roman", 25, FontStyle.Bold), Brushes.Black, 220, 620)

        'e.HasMorePages = False
        ' ---------------------------------------------------------------------set fonts----------------------------------------------------------------------------
        '  Dim f8 As New Font("Arial", 8, FontStyle.Regular)
        ' Dim f8b As New Font("Verdana", 8, FontStyle.Regular)

        'set alignments
        ' Dim left As New StringFormat
        ' Dim center As New StringFormat
        ' Dim right As New StringFormat


        '  left.Alignment = StringAlignment.Near
        ' center.Alignment = StringAlignment.Center
        ' right.Alignment = StringAlignment.Far

        'draw rectangles
        ' Dim Rect1 As New Rectangle(5, 5, 240, 17)
        ' Dim Rect2 As New Rectangle(5, 22, 240, 17)
        ' Dim Rect3 As New Rectangle(5, 39, 240, 17)


        '  e.Graphics.DrawRectangle(Pens.Black, Rect1)
        '  e.Graphics.DrawRectangle(Pens.Black, Rect2)
        '  e.Graphics.DrawRectangle(Pens.Black, Rect3)

        '  e.Graphics.DrawString("Grocery Shop", f8, Brushes.Black, Rect1, center)
        '   e.Graphics.DrawString("Raipur", f8, Brushes.Black, Rect2, center)
        '   e.Graphics.DrawString("1234567895", f8, Brushes.Black, Rect3, center)

        '  Dim Rect4 As New Rectangle(5, 73, 47, 17)
        ' Dim Rect5 As New Rectangle(52, 73, 47, 17)
        ' Dim Rect6 As New Rectangle(99, 73, 47, 17)
        ' Dim Rect7 As New Rectangle(146, 73, 47, 17)
        ' Dim Rect8 As New Rectangle(193, 73, 47, 17)


        ' e.Graphics.DrawRectangle(Pens.Black, Rect4)
        ' e.Graphics.DrawRectangle(Pens.Black, Rect5)
        ' e.Graphics.DrawRectangle(Pens.Black, Rect6)
        'e.Graphics.DrawRectangle(Pens.Black, Rect7)
        'e.Graphics.DrawRectangle(Pens.Black, Rect8)

        'e.Graphics.DrawString("Items", f8, Brushes.Black, Rect4, center)
        'e.Graphics.DrawString("ID", f8, Brushes.Black, Rect5, center)
        'e.Graphics.DrawString("Price", f8, Brushes.Black, Rect6, center)
        ' e.Graphics.DrawString("Quantity", f8, Brushes.Black, Rect7, center)
        '  e.Graphics.DrawString("Total", f8, Brushes.Black, Rect8, center)

        ''
        '  Dim Rect9 As New Rectangle(5, 90, 47, 17)
        '  Dim Rect10 As New Rectangle(52, 90, 47, 17)
        '  Dim Rect11 As New Rectangle(99, 90, 47, 17)
        '  Dim Rect12 As New Rectangle(146, 90, 47, 17)
        '  Dim Rect13 As New Rectangle(193, 90, 47, 17)


        'e.Graphics.DrawRectangle(Pens.Black, Rect9)
        'e.Graphics.DrawRectangle(Pens.Black, Rect10)
        ' e.Graphics.DrawRectangle(Pens.Black, Rect11)
        ' e.Graphics.DrawRectangle(Pens.Black, Rect12)
        '  e.Graphics.DrawRectangle(Pens.Black, Rect13)

        '   e.Graphics.DrawString(DataGridView1.Rows(0).Cells(0).Value, f8, Brushes.Black, Rect9, center)
        '  e.Graphics.DrawString(DataGridView1.Rows(0).Cells(1).Value, f8, Brushes.Black, Rect10, center)
        ' e.Graphics.DrawString(DataGridView1.Rows(0).Cells(2).Value, f8, Brushes.Black, Rect11, center)
        ' e.Graphics.DrawString(DataGridView1.Rows(0).Cells(3).Value, f8, Brushes.Black, Rect12, center)
        ' e.Graphics.DrawString(DataGridView1.Rows(0).Cells(4).Value, f8, Brushes.Black, Rect13, center)
        '  -------------------------------------------------------------------------------------------------------------------------------------------------------------------    

        '**********************************************************************************************************************************************
        ''Set the title and style for the bill
        '   e.Graphics.DrawString("Grocery Shop", New Font("Arial", 50), Brushes.BlueViolet, 210, 35)
        ' e.Graphics.DrawString("Your Bill " + ClNameTb.Text.ToString, New Font("Arial", 25), Brushes.BlueViolet, 220, 110)

        'Set the starting position for the DataGridView content
        '  e.Graphics.DrawString(DataGridView1.Rows(0).Cells(0).Value, New Font("Arial", 50), Brushes.BlueViolet, 210, 35)
        '  e.Graphics.DrawString(DataGridView1.Rows(0).Cells(1).Value, New Font("Arial", 50), Brushes.BlueViolet, 210, 35)
        ' e.Graphics.DrawString(DataGridView1.Rows(0).Cells(2).Value, New Font("Arial", 50), Brushes.BlueViolet, 210, 35)
        'e.Graphics.DrawString(DataGridView1.Rows(0).Cells(3).Value, New Font("Arial", 50), Brushes.BlueViolet, 210, 35)
        'e.Graphics.DrawString(DataGridView1.Rows(0).Cells(4).Value, New Font("Arial", 50), Brushes.BlueViolet, 210, 35)

        'Dim bm As New Bitmap(Me.DataGridView1.Width, Me.DataGridView1.Height)
        ' DataGridView1.DrawToBitmap(bm, New Rectangle(10, 10, Me.DataGridView1.Width, Me.DataGridView1.Height))
        ' e.Graphics.DrawImage(bm, 70, 170)


        'Display the total amount on the bill
        ' e.Graphics.DrawString("Total Amount Rs " + GrdTotal.ToString, New Font("Arial", 20), Brushes.Crimson, 305, 535)

        'Display a thank you message
        ' e.Graphics.DrawString("********* Thanks For Buying In Our Shop *********", New Font("Arial", 20), Brushes.Crimson, 120, 575)
        'e.HasMorePages = False
        '******************************************************************************************************************************************************
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim obj = New Login
        obj.Show()
        Me.Hide()

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        TotaLbill.Text = "Total"
        Guna2DataGridView1.Rows.Clear()

    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        DisplayItem()
    End Sub

    Private Sub FilterByCat()
        con.open()
        Dim query = "select * from ItemTbl  where Itcat= '" & SearchCb.SelectedItem.ToString() & "'"
        Dim cmd = New SqlCommand(query, con)
        Dim adapter As SqlDataAdapter
        adapter = New SqlDataAdapter(cmd)
        Dim builder As New SqlCommandBuilder(adapter)
        Dim ds As DataSet
        ds = New DataSet
        adapter.Fill(ds)
        ItemDGV.DataSource = ds.Tables(0)
        con.close()
    End Sub
    Private Sub SearchCb_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles SearchCb.SelectionChangeCommitted
        FilterByCat()
    End Sub

    Private bitmap As Bitmap

    Private Sub PrintToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PrintToolStripMenuItem1.Click
        Dim height As Integer = Guna2DataGridView1.Height
        Guna2DataGridView1.Height = Guna2DataGridView1.RowCount * Guna2DataGridView1.RowTemplate.Height
        bitmap = New Bitmap(Me.Guna2DataGridView1.Width, Me.Guna2DataGridView1.Height)
        Guna2DataGridView1.DrawToBitmap(bitmap, New Rectangle(0, 0, Me.Guna2DataGridView1.Width, Me.Guna2DataGridView1.Height))
        PrintPreviewDialog1.PrintPreviewControl.Zoom = 0.7
        PrintPreviewDialog1.ShowDialog()
        Guna2DataGridView1.Height = height
    End Sub

    Private Sub PictureBox1_Click_1(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim iExit As DialogResult
        iExit = MessageBox.Show("Confirm if you want to exit", "Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If iExit = Windows.Forms.DialogResult.Yes Then
            Application.Exit()
        End If

    End Sub
End Class