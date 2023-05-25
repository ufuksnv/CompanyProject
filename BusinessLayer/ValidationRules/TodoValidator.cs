using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class TodoValidator : AbstractValidator<ToDo>
    {
        public TodoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Lütfen Başlığı Boş Bırakmayınız!");
            RuleFor(x => x.Text).NotEmpty().WithMessage("Lütfen İçeriği Boş Bırakmayınız!");
        }

    }
}
