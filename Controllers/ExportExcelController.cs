using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    public class ExportExcelController : Controller
    {
        private readonly IBangKeChiPhiSevices _bangKeChiPhiSevices;
        public ExportExcelController(IBangKeChiPhiSevices bangKeChiPhiSevices)
        {
            _bangKeChiPhiSevices = bangKeChiPhiSevices;
        }
        public async Task<IActionResult> BangKeChiPhiExcel(string id, string loai)
        {
            string hoten = String.Empty;
            using (var workbook = new XLWorkbook())
            {
                var bangke = await _bangKeChiPhiSevices.GetBangKeChiPhi(id, loai, "2");
                var worksheet = workbook.Worksheets.Add("Bảng kê viện phí");
                var currentRow = 1;
                var sott = 0;
                worksheet.Cell(currentRow, 1).Value = "STT";
                worksheet.Cell(currentRow, 2).Value = "Mã bệnh nhân";
                worksheet.Cell(currentRow, 3).Value = "Họ tên";
                worksheet.Cell(currentRow, 4).Value = "Ngày sinh";
                worksheet.Cell(currentRow, 5).Value = "Giới tính";
                worksheet.Cell(currentRow, 6).Value = "Chẩn đoán";
                worksheet.Cell(currentRow, 7).Value = "Nhóm dịch vụ";
                worksheet.Cell(currentRow, 8).Value = "Nội dung";
                worksheet.Cell(currentRow, 9).Value = "Đơn vị";
                worksheet.Cell(currentRow, 10).Value = "Đơn giá";
                worksheet.Cell(currentRow, 11).Value = "Số lượng";
                worksheet.Cell(currentRow, 12).Value = "Số lượng thanh toán";
                worksheet.Cell(currentRow, 13).Value = "Thành tiền";
                worksheet.Cell(currentRow, 14).Value = "BHYT chi trả";
                worksheet.Cell(currentRow, 15).Value = "BHTN thanh toán";
                worksheet.Cell(currentRow, 16).Value = "KH chi trả";
                worksheet.Cell(currentRow, 17).Value = "Số biên lai";
                worksheet.Cell(currentRow, 18).Value = "Mã vào viện";
                worksheet.Cell(currentRow, 19).Value = "Mã quản lý";
                worksheet.Row(currentRow).Style.Font.SetBold();
                foreach (var item in bangke)
                {
                    currentRow++;
                    if (!String.IsNullOrEmpty(item.id))
                    {
                        sott++;
                        worksheet.Cell(currentRow, 1).Value = sott;
                        worksheet.Cell(currentRow, 2).Value = item.mabn.ToString();
                        worksheet.Cell(currentRow, 3).Value = item.hoten.ToString();
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDateTime(item.ngaysinh, CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                        worksheet.Cell(currentRow, 5).Value = item.gioitinh.ToString();
                        worksheet.Cell(currentRow, 6).Value = item.chandoan.ToString();
                        worksheet.Cell(currentRow, 7).Value = (item.nhomvp ?? "").ToString();
                        worksheet.Cell(currentRow, 7).Style.Alignment.WrapText = true;
                        worksheet.Cell(currentRow, 8).Value = (item.ten ?? "").ToString();
                        worksheet.Cell(currentRow, 8).Style.Alignment.WrapText = true;
                        worksheet.Cell(currentRow, 9).Value = (item.dvt ?? "").ToString();
                        worksheet.Cell(currentRow, 10).Value = item.dongia;
                        worksheet.Cell(currentRow, 10).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 11).Value = item.soluong;
                        worksheet.Cell(currentRow, 11).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "0";
                        worksheet.Cell(currentRow, 12).Value = item.SoLuongTT;
                        worksheet.Cell(currentRow, 12).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 12).Style.NumberFormat.Format = "0";
                        worksheet.Cell(currentRow, 13).Value = item.thanhtien;
                        worksheet.Cell(currentRow, 13).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 13).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 14).Value = item.bhyttra;
                        worksheet.Cell(currentRow, 14).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 14).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 15).Value = item.bhtn;
                        worksheet.Cell(currentRow, 15).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 15).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 16).Value = item.bntra;
                        worksheet.Cell(currentRow, 16).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 16).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 17).Value = item.sobienlai;
                        worksheet.Cell(currentRow, 17).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 17).Style.NumberFormat.Format = "0";
                        worksheet.Cell(currentRow, 18).Value = item.mavaovien;
                        worksheet.Cell(currentRow, 18).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 18).Style.NumberFormat.Format = "0";
                        worksheet.Cell(currentRow, 19).Value = item.maql;
                        worksheet.Cell(currentRow, 19).DataType = XLDataType.Number;
                        worksheet.Cell(currentRow, 19).Style.NumberFormat.Format = "0";
                    }
                    else
                    {
                        if (item.thanhtien > 0)
                        {
                            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 12)).Merge();
                            worksheet.Cell(currentRow, 1).Value = (item.ten ?? "").ToString();
                            worksheet.Cell(currentRow, 13).Value = item.thanhtien;
                            worksheet.Cell(currentRow, 13).DataType = XLDataType.Number;
                            worksheet.Cell(currentRow, 13).Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell(currentRow, 16).Value = item.bntra;
                            worksheet.Cell(currentRow, 16).DataType = XLDataType.Number;
                            worksheet.Cell(currentRow, 16).Style.NumberFormat.Format = "#,##0";
                            worksheet.Row(currentRow).Style.Font.SetBold();
                        }
                        else
                        {
                            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 19)).Merge();
                            worksheet.Cell(currentRow, 1).Value = (item.ten ?? "").ToString();
                            worksheet.Row(currentRow).Style.Font.SetBold().Font.SetItalic();
                        }
                    }
                    hoten = item.hoten;
                }
                worksheet.Columns().AdjustToContents();
                worksheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"BangKeVienPhi_{hoten}.xlsx");
                }
            }
        }
    }
}
