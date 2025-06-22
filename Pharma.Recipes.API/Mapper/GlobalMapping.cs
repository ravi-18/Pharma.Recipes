using AutoMapper;
using Pharma.Recipes.API.Dtos.Steps;
using Pharma.Recipes.API.Models;

namespace Pharma.Recipes.API.Mapper
{
    public class GlobalMapping : Profile
    {
        public GlobalMapping()
        {
            CreateMap<StepDetailDto, Step>();
            CreateMap<StepCreateDto, Step>();
            CreateMap<StepParameterCreateDto, StepParameter>();
            CreateMap<StepParameterDto, StepParameter>();
        }
    }
}
