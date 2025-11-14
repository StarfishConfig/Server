namespace Nerosoft.Starfish.Transit;

/// <summary>
/// Data transfer object for representing the result of a creation operation.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CreatedResultDto<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreatedResultDto{T}"/> class.
	/// </summary>
	public CreatedResultDto()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CreatedResultDto{T}"/> class with the specified identifier.
	/// </summary>
	/// <param name="id"></param>
	public CreatedResultDto(T id)
		: this()
	{
		Id = id;
	}

	/// <summary>
	/// Gets or sets the identifier of the created entity.
	/// </summary>
	public T Id { get; set; }
}
