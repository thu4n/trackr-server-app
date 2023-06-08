using TestTestServer.Data;
using Microsoft.AspNetCore.Mvc;
using TestTestServer.Models;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace TestTestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForgotPasswordController :Controller
    {
        private readonly EsistAccountService _esistAccountService;
        public ForgotPasswordController( EsistAccountService esistAccountService)
        {
            _esistAccountService = esistAccountService;
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(LoginCheck login)
        {
            //kiểm tra xem account có tồn tại hay không
            var check = await _esistAccountService.checkAccount(login);
            if (check.Account == null) { return NotFound(); }

            // gửi mail trả về mật khẩu cho ng dùng
            var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true); // smtp host, port, use ssl.
            client.Authenticate("TrackrService@gmail.com", "dtwvjgfkeyypoliw"); // gmail account, app password
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Trackr", "TrackrService@gmail.com"));
            message.To.Add(new MailboxAddress("", login.Account));
            message.Subject = "Forgot Password Trackr ";
            Random rd = new Random();
            int Numrd = rd.Next(100000, 999999);
            string text = "Code Reset Password is : " + Numrd.ToString(); // gửi code otp qua gmail
            string msg = "";
            // mã hóa code OTP
            using (SHA256 sha256 = SHA256.Create())
            {
                // Cần chuyển đổi string sang dạng byte khi Hash
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(Numrd.ToString()));
                // Chuyển đổi chuỗi vừa hash sang dạng string để dễ sử dụng
                 msg = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }
            // 
            Otp otp = new Otp
            {
                Id = check.ID,
                name = check.Name,
                Code = msg,
                role = check.role,
            };
            message.Body = new TextPart("plain")
            {
                Text = text 
            };
            client.Send(message);

            return Ok(otp);
        }
    }
}
