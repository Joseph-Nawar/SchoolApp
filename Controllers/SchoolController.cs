using Microsoft.AspNetCore.Mvc;
using SchoolApp.Models;
using SchoolApp.Data;
using SchoolApp.Repository;
using SchoolApp.Interface;
using AutoMapper;
using SchoolApp.Dto;
using System.Diagnostics.Metrics;

namespace SchoolApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : Controller
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public SchoolController(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<School>))]
        public IActionResult GetSchools()
        {
            var schools = _mapper.Map<List<SchoolDto>>(_schoolRepository.GetSchools());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(schools);
        }

        [HttpGet("{schoolId}")]
        [ProducesResponseType(200, Type = typeof(School))]
        [ProducesResponseType(400)]
        public IActionResult GetSchool(int schoolId)
        {
            if (!_schoolRepository.SchoolExists(schoolId))
                return NotFound();

            var school = _mapper.Map<SchoolDto>(_schoolRepository.GetSchool(schoolId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(school);
        }

        [HttpGet("/{teacherId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(School))]
        public IActionResult GetSchoolByTeacher(int teacherId)
        {
            var school = _mapper.Map<SchoolDto>(
                _schoolRepository.GetSchoolByTeacher(teacherId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(school);
        }

        [HttpGet("/{studentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(School))]
        public IActionResult GetSchoolByStudent(int studentId)
        {
            var school = _mapper.Map<SchoolDto>(
                _schoolRepository.GetSchoolByStudent(studentId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(school);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSchool([FromBody] SchoolDto schoolCreate)
        {
            if (schoolCreate == null)
                return BadRequest(ModelState);

            var school = _schoolRepository.GetSchools()
                .Where(c => c.Name.Trim().ToUpper() == schoolCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (school != null)
            {
                ModelState.AddModelError("", "School already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schoolMap = _mapper.Map<School>(schoolCreate);

            if (!_schoolRepository.CreateSchool(schoolMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("School is created Successfully");
        }


        [HttpPut("{schoolId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int schoolId, [FromBody] SchoolDto updatedSchool)
        {
            if (updatedSchool == null)
                return BadRequest(ModelState);

            if (schoolId != updatedSchool.Id)
                return BadRequest(ModelState);

            if (!_schoolRepository.SchoolExists(schoolId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var schoolMap = _mapper.Map<School>(updatedSchool);

            if (!_schoolRepository.UpdateSchool(schoolMap))
            {
                ModelState.AddModelError("", "Updating School went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{schoolId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSchool(int schoolId)
        {
            if (!_schoolRepository.SchoolExists(schoolId))
            {
                return NotFound();
            }

            var schoolToDelete = _schoolRepository.GetSchool(schoolId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_schoolRepository.DeleteSchool(schoolToDelete))
            {
                ModelState.AddModelError("", "Deleting School went wrong");
            }
            return NoContent();
        }

    }
}
