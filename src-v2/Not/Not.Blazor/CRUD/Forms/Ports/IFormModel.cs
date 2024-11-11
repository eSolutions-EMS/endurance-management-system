namespace Not.Blazor.CRUD.Forms.Ports;

public interface IFormModel<T>
{
    void FromEntity(T entity);
}
