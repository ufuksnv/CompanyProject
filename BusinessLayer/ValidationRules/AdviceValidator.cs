using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class AdviceValidator : AbstractValidator<Advice>
    {
        public AdviceValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Lütfen Doktor Adını Boş Bırakmayınız!");
            RuleFor(x => x.Text).NotEmpty().WithMessage("Lütfen Randevu Tarihini Boş Bırakmayınız!");
        }
    }
}
