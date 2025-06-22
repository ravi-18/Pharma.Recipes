using AutoMapper;
using Pharma.Recipes.API.Dtos.Steps;
using Pharma.Recipes.API.Models;

namespace Pharma.Recipes.API.Mapper
{
    public class GlobalMapping : Profile
    {
        public GlobalMapping()
        {
            //CreateMap<Step, StepCreateDto>();
            CreateMap<StepCreateDto, Step>();
            //CreateMap<StepParameter, StepParameterCreateDto>();
            CreateMap<StepParameterCreateDto, StepParameter>();
        }
    }
}
