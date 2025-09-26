using FluentValidation;
using WXEnvironment.AFScreen.Data;

#pragma warning disable CS8602 // Possible null reference argument.

namespace WXEnvironment.AFScreen.Validator
{
    /// <summary>
    /// 
    /// </summary>
    public class AFScreenSettingValidator : AbstractValidator<AFScreenSettingModel>
    {
        /// <summary>
        /// 
        /// </summary>
        public AFScreenSettingValidator()
        {
            RuleFor(x => x.InfoId).NotEmpty().WithMessage("“编号”不能为空");
            RuleFor(x => x.BgType).NotEmpty().WithMessage("“背景类型”不能为空")
                .Must(bgType => bgType.Equals("color", StringComparison.OrdinalIgnoreCase) || bgType.Equals("image", StringComparison.OrdinalIgnoreCase))
                .WithMessage("“背景类型”只能是 'color' 或 'image'");
            RuleFor(x => x.BelongUserId).NotEmpty().WithMessage("“所属用户”不能为空").Length(24).WithMessage("“所属用户”格式不正确");
            RuleFor(x => x.BelongUserName).NotEmpty().WithMessage("“所属用户名称”不能为空");

           
        }
    }
}
