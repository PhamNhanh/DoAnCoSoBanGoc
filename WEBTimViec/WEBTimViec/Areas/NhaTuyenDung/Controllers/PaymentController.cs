using Microsoft.AspNetCore.Mvc;
using WEBTimViec.Models.VNPay;
using WEBTimViec.Services.VnPay;

namespace WEBTimViec.Areas.NhaTuyenDung.Controllers
{
    [Area("NhaTuyenDung")]
    public class PaymentController : Controller
    {

        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }


    }

}
