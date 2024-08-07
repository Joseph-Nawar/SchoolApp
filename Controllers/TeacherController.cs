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
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;

        private readonly IMapper _mapper;

        public TeacherController(ITeacherRepository teacherRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Teacher>))]
        public IActionResult GetTeachers()
        {
            var teachers = _mapper.Map<List<TeacherDto>>(_teacherRepository.GetTeachers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(teachers);
        }

        [HttpGet("{teacherId}")]
        [ProducesResponseType(200, Type = typeof(Teacher))]
        [ProducesResponseType(400)]
        public IActionResult GetTeacher(int teacherId)
        {
            if (!_teacherRepository.TeacherExists(teacherId))
                return NotFound();

            var teacher = _mapper.Map<TeacherDto>(_teacherRepository.GetTeacher(teacherId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(teacher);
        }

        //move to student controller
        [HttpGet("GetStudentByTeacher/{teacherId}")]
        [ProducesResponseType(200, Type = typeof(Teacher))]
        [ProducesResponseType(400)]
        public IActionResult GetStudentByTeacher(int teacherId)
        {
            if (!_teacherRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var teacher = _mapper.Map<List<StudentDto>>(
                _teacherRepository.GetStudentsByTeacher(teacherId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(teacher);
        }

        [HttpGet("GetTeacherByStudent/{studentId}")]
        [ProducesResponseType(200, Type = typeof(Teacher))]
        [ProducesResponseType(400)]
        public IActionResult GetTeacherByStudent(int studentId)
        {
            if (!_studentRepository.StudentExists(studentId))
            {
                return NotFound();
            }

            var student = _mapper.Map<List<StudentDto>>(
                _teacherRepository.GetTeacherByStudent(studentId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTeacher([FromBody] TeacherDto teacherCreate)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the incoming DTO is null
            if (teacherCreate == null)
                return BadRequest("Teacher data is null");

            // Check if the teacher already exists
            var existingTeacher = _teacherRepository.GetTeachers()
                .FirstOrDefault(t => t.Name?.Trim().ToUpper() == teacherCreate.Name?.Trim().ToUpper());

            if (existingTeacher != null)
            {
                ModelState.AddModelError("", "Teacher already exists");
                return StatusCode(422, ModelState);
            }

            // Map the DTO to the entity
            Teacher teacherMap;
            try
            {
                teacherMap = _mapper.Map<Teacher>(teacherCreate);
            }
            catch (AutoMapperMappingException ex)
            {
                ModelState.AddModelError("", $"Mapping failed: {ex.Message}");
                return StatusCode(500, ModelState);
            }

            // Create the new teacher
            if (!_teacherRepository.CreateTeacher(teacherMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving the teacher");
                return StatusCode(500, ModelState);
            }

            return Ok("Teacher successfully created");
        }

        [HttpPut("{teacherId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTeacher(int teacherId, [FromBody] TeacherDto updateTeacher)
        {
            if (updateTeacher == null)
                return BadRequest(ModelState);

            if (teacherId != updateTeacher.Id)
                return BadRequest(ModelState);

            if (!_teacherRepository.TeacherExists(teacherId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var teachMap = _mapper.Map<Teacher>(updateTeacher);

            if (!_teacherRepository.UpdateTeacher(teachMap))
            {
                ModelState.AddModelError(" ", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{teacherId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTeacher(int teacherId)
        {
            if (!_teacherRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var teacherToDelete = _teacherRepository.GetTeacher(teacherId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_teacherRepository.DeleteTeacher(teacherToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the teacher");
            }

            return NoContent();
        }
    }
}
