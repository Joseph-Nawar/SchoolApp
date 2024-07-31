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
        private readonly IMapper _mapper;

        public TeacherController(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
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
            if(!_teacherRepository.TeacherExists(teacherId))
                return NotFound();

            var teacher = _mapper.Map<TeacherDto>(_teacherRepository.GetTeacher(teacherId));

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(teacher);
        }

        [HttpGet("{teacherId}/student")]
        [ProducesResponseType(200, Type = typeof(Teacher))]
        [ProducesResponseType(400)]
        public IActionResult GetStudentByTeacher(int teacherId)
        {
            if (!_teacherRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var teacher = _mapper.Map<List<StudentDto>>(
                _teacherRepository.GetStudentByTeacher(teacherId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(teacher);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTeacher([FromBody] TeacherDto teacherCreate) 
        {
            if(teacherCreate == null)
                return BadRequest(ModelState);

            var teachers = _teacherRepository.GetTeachers()
                .Where(t => t.Name.Trim().ToUpper() == teacherCreate.Name.Trim().ToUpper())
                .FirstOrDefault();
            
            if(teachers != null)
            {
                ModelState.AddModelError(" ", "Teacher already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var teacherMap = _mapper.Map<Teacher>(teacherCreate);

            if (!_teacherRepository.CreateTeacher(teacherMap))
            {
                ModelState.AddModelError(" ", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{teacherId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTeacher(int teacherId, [FromBody] TeacherDto updateTeacher)
        {
            if(updateTeacher == null)
                return BadRequest(ModelState);

            if(teacherId != updateTeacher.Id)
                return BadRequest(ModelState);

            if(!_teacherRepository.TeacherExists(teacherId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var teachMap = _mapper.Map<Teacher>(updateTeacher);

            if(!_teacherRepository.UpdateTeacher(teachMap))
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

            if(!_teacherRepository.DeleteTeacher(teacherToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the teacher");
            }

            return NoContent();
        }
    }
}
