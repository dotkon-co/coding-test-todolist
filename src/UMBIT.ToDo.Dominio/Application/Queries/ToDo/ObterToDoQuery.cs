﻿using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.Dominio.Entidades.ToDo;

namespace UMBIT.ToDo.Dominio.Application.Queries.ToDo
{
    public class ObterToDoQuery : UMBITQuery<ObterToDoQuery, IQueryable<ToDoItem>>
    {
        protected override void Validadors(ValidatorQuery<ObterToDoQuery> validator)
        {
        }
    }
}