﻿using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Localization.Dto
{
    public class GetLanguageForEditOutput
    {
        public ApplicationLanguageEditDto Language { get; set; }

        public List<ComboboxItemDto> LanguageNames { get; set; }

        public List<ComboboxItemDto> Flags { get; set; }

        public GetLanguageForEditOutput()
        {
            LanguageNames = new List<ComboboxItemDto>();
            Flags = new List<ComboboxItemDto>();
        }
    }
}