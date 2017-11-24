using System.Collections.Generic;
using SAM.BusinessLogic.Ingenieria;

namespace SAM.BusinessLogic
{
    public delegate void DelegateValidacionIngenieria(IRegistroValido registroValido);

    public delegate void DelegateConstruccionRegistroIngenieria(IRegistroValido registroValido);

    public delegate bool DelegateFuncionValidacionRegistro(IRegistroValido registroValido);

    public delegate bool DelegateFuncionValidacionIntegridadRegistro(IRegistroValido registroValido);

}
