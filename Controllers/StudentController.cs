using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;
using SchoolApp.Data;
using SchoolApp.Repository;
using SchoolApp.Interface;
using AutoMapper;
using SchoolApp.Dto;

namespace SchoolApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StudentDto>))]
        public IActionResult GetStudents()
        {
            var students = _mapper.Map<List<StudentDto>>(_studentRepository.GetStudents());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(students);
        }

        [HttpGet("{studentId}")]
        [ProducesResponseType(200, Type = typeof(StudentDto))]
        [ProducesResponseType(400)]
        public IActionResult GetStudent(int studentId)
        {
            if (!_studentRepository.StudentExists(studentId))
                return NotFound();

            var student = _mapper.Map<StudentDto>(_studentRepository.GetStudent(studentId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateStudent([FromBody] StudentDto studentCreate)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the incoming DTO is null
            if (studentCreate == null)
                return BadRequest("Student data is null");

            // Check if the student already exists
            var existingStudent = _studentRepository.GetStudents()
                .FirstOrDefault(s => s.Name?.Trim().ToUpper() == studentCreate.Name?.Trim().ToUpper());

            if (existingStudent != null)
            {
                ModelState.AddModelError("", "Student already exists");
                return StatusCode(422, ModelState);
            }

            // Map the DTO to the entity
            Student studentMap;
            try
            {
                studentMap = _mapper.Map<Student>(studentCreate);
            }
            catch (AutoMapperMappingException ex)
            {
                ModelState.AddModelError("", $"Mapping failed: {ex.Message}");
                return StatusCode(500, ModelState);
            }

            // Create the new student
            if (!_studentRepository.CreateStudent(studentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving the student");
                return StatusCode(500, ModelState);
            }

            return Ok("Student successfully created");
        }

        [HttpPut("{studentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStudent(int studentId, [FromBody] StudentDto updateStudent)
        {
            if (updateStudent == null)
                return BadRequest(ModelState);

            if (studentId != updateStudent.Id)
                return BadRequest(ModelState);

            if (!_studentRepository.StudentExists(studentId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var studentMap = _mapper.Map<Student>(updateStudent);

            if (!_studentRepository.UpdateStudent(studentMap))
            {
                ModelState.AddModelError(" ", "Something went wrong updating student");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{studentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStudent(int studentId)
        {
            if (!_studentRepository.StudentExists(studentId))
            {
                return NotFound();
            }

            var studentToDelete = _studentRepository.GetStudent(studentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_studentRepository.DeleteStudent(studentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the student");
            }

            return NoContent();
        }
    }
}
