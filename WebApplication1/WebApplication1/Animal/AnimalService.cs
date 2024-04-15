namespace WebApplication1.Animal;

public interface IAnimalService
{
    public IEnumerable<Animal> GetAnimals(string orderBy);
    public bool CreateAnimal(CreateAnimalDTO dto);
}

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    public AnimalService(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }
    
    public IEnumerable<Animal> GetAnimals(string orderBy)
    {
        return _animalRepository.FetchAllAnimals(orderBy);
    }

    public bool CreateAnimal(CreateAnimalDTO dto)
    {
        return _animalRepository.CreateAnimal(dto.Name);
    }
}