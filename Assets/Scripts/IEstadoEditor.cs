//La clase base de los estados
//Todos los estados tendrán lo que dice aqui
//Es una plantilla de los estados
public interface IEstadoEditor 
{
    void Entrar(EditorStateMachine ModoCrear);
    void Ejecutar(EditorStateMachine ModoCrear);
    void Salir(EditorStateMachine ModoCrear);

}