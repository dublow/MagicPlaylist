using FluentValidation;
using MagicPlaylist.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Web.Validators
{
    public class DeezerModelValidator : AbstractValidator<DeezerModel>
    {
        public DeezerModelValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("Empty userId");
        }
    }
}
