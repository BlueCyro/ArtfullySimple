namespace ArtfullySimple;

public class ArtOpCodeAttribute(ArtOp opcode, Type t) : Attribute
{
    public readonly ArtOp Op = opcode;
    public readonly Type Type = t;
}