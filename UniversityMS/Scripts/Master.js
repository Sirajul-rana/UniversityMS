$(document).ready(function () {
    //Sirajuls code starts from here
    var tempArray;
    //Adding extra credit for teacher
    function addExtraCredit(extraCredit, teacherId) {
        var urlExtra = "/Admin/UpdateTeacherTakenCredit/";
        $.ajax({
            url: urlExtra,
            data: {
                extraCredit: extraCredit,
                teacherId: teacherId
            },
            cache: false,
            type: "POST",
            success: function (data) {
                alertify.set('notifier', 'position', 'top-right');
                alertify.success(data);
            },
            error: function (reponse) {
                alert("error-teacher: " + reponse);
            }
        });
    }

    //Loading teacher dropdownlist
    function getTeacher(url, data) {
        $.ajax({
            url: url,
            data: { departmentId: data },
            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value=''>Select Teacher</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#TeacherId").html(markup).show();
            },
            error: function (reponse) {
                alert("error-teacher: " + reponse);
            }
        });
    }
    //Loading course dropdownlist
    function getCourse(url, data) {
        $.ajax({
            url: url,
            data: { departmentId: data },
            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value=''>Select Course Code</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#CourseId").html(markup).show();
            },
            error: function (reponse) {
                alert("error-course: " + reponse);
            }
        });
    }

    //Changing Teacher and Course dropdownlist
    $("#DepartmentId").change(function () {
        if ($("#DepartmentId").val() === "") {
            $("#TeacherId").find('option:not(:first)').remove();
            $("#CourseId").find('option:not(:first)').remove();
            $("#creditTobeTakenTextBox").val("");
            $("#courseNameTextBox").val("");
            $("#TakenCourseCredit").val("");
            $("#remainingCreditTextBox").val("");
        } else {
            var urlTeacher = "/Admin/GetTeacherByDepartment/";
            var urlCourse = "/Admin/GetCourseByDepartment/";
            var data = $("#DepartmentId").val();

            getTeacher(urlTeacher, data);
            getCourse(urlCourse, data);
        }

    });

    //Action against Teacher dropdownlist
    $("#TeacherId").change(function () {
        if ($("#TeacherId").val() === "") {
            $("#creditTobeTakenTextBox").val("");
            $("#remainingCreditTextBox").val("");
        } else {
            var urlTeacher = "/Admin/GetTeacherById/";
            var data = $("#TeacherId").val();

            $.ajax({
                url: urlTeacher,
                data: { teacherId: data },
                cache: false,
                type: "POST",
                success: function (data) {
                    $("#creditTobeTakenTextBox").val(data.TakenCredit);
                    $("#remainingCreditTextBox").val(data.TakenCredit - data.RemainingCredit);
                    $("#CourseId").prop('selectedIndex', 0);
                    $("#courseNameTextBox").val("");
                    $("#courseCreditTextBox").val("");
                },
                error: function (reponse) {
                    alert("error-Teacher-one: " + reponse);
                }
            });
        }

    });


    //Action against Course dropdownlist
    $("#CourseId").change(function () {
        if ($("#CourseId").val() === "") {
            $("#courseNameTextBox").val("");
            $("#TakenCourseCredit").val("");

        } else {
            var urlCourse = "/Admin/GetCourseById/";
            var data = $("#CourseId").val();

            $.ajax({
                url: urlCourse,
                data: { courseId: data },
                cache: false,
                type: "POST",
                success: function (data) {
                    $("#courseNameTextBox").val(data.CourseName);
                    $("#TakenCourseCredit").val(data.Credit);

                },
                error: function (reponse) {
                    alert("error-Course-one: " + reponse);
                }
            });
        }

    });

    //Showing assigned course teachers
    $("#departmentDropdown").change(function () {
        var deparmentId = $("#departmentDropdown").val();
        if (deparmentId === "") {
            $("#tableBody").empty();
        }
        else {
            var url = "/admin/ViewCourseAssign/";
            var data = {
                departmentId: deparmentId
            };
            $.ajax({
                url: url,
                data: JSON.stringify(data),
                cache: false,
                type: "POST",
                contentType: "application/json",
                success: function (data) {
                    var tblHtml = "";
                    jQuery.each(data, function (i, val) {
                        tblHtml += "<tr><td>" + val.Course.CourseCode + "</td>";
                        tblHtml += "<td>" + val.Course.CourseName + "</td>";
                        tblHtml += "<td>" + val.Semester.SemesterName + "</td>";
                        tblHtml += "<td>" + val.Teacher.TeacherName + "</td></tr>";
                    });

                    $("#tableBody").html(tblHtml);
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                }
            });
        }

    });



    $("#department").change(function () {
        var deparmentId = $("#department").val();
        if (deparmentId === "") {
            $("#viewTableBody").empty();
        }
        else {
            var url = "/admin/ViewSchedule/";
            var data = {
                departmentId: deparmentId
            };
            $.ajax({
                url: url,
                data: JSON.stringify(data),
                cache: false,
                type: "POST",
                contentType: "application/json",
                success: function (data) {
                    var tblHtml = "";
                    jQuery.each(data, function (i, val) {
                        tblHtml += "<tr><td>" + val.CourseCode + "</td>";
                        tblHtml += "<td>" + val.CourseName + "</td>";
                        tblHtml += "<td>"
                            + val.Schedule +
                            "</td></tr>";
                    });

                    $("#viewTableBody").html(tblHtml);
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                }
            });
        }

    });

    //Viewing student result
    $("#StudentId").change(function () {
        if ($("#StudentId").val() === "") {
            $("#tableBody").empty();
            $("#StudentName").val("");
            $("#StudentEmail").val("");
            $("#departmentId").val("");
        }
        else {
            var studentId = $("#StudentId").val();
            var url = "/student/GetStudentInfo/";
            var data = {
                studentId: studentId
            };
            $.ajax({
                url: url,
                data: JSON.stringify(data),
                cache: false,
                type: "POST",
                contentType: "application/json",
                success: function (data) {
                    $("#StudentName").val(data.StudentName);
                    $("#StudentEmail").val(data.StudentEmail);
                    $("#departmentId").val(data.Department.DepartmentName);

                    //Getting course and grade information
                    var urlCourse = "/student/GetCourses/";
                    var dataCourse = {
                        studentId: data.StudentId
                    };
                    $.ajax({
                        url: urlCourse,
                        data: JSON.stringify(dataCourse),
                        cache: false,
                        type: "POST",
                        contentType: "application/json",
                        success: function (data) {
                            tempArray = data;
                            var tblHtml = "";
                            jQuery.each(data, function (i, val) {
                                tblHtml += "<tr><td>" + val.CourseCode + "</td>";
                                tblHtml += "<td>" + val.CourseName + "</td>";
                                tblHtml += "<td>" + val.Grade.GradeCode + "</td></tr>";
                            });

                            $("#tableBody").html(tblHtml);
                        },
                        error: function (reponse) {
                            alert("error : " + reponse);
                        }
                    });
                    //var tblHtml = "";
                    //jQuery.each(data, function (i, val) {
                    //    tblHtml += "<tr><td>" + val.StudentName + "</td>";
                    //    tblHtml += "<td>" + val.Course.CourseName + "</td>";
                    //    tblHtml += "<td>" + val.Semester.SemesterName + "</td>";
                    //    tblHtml += "<td>" + val.Teacher.TeacherName + "</td></tr>";
                    //});

                    //$("#tableBody").html(tblHtml);
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                }
            });
        }

    });


    //From time picker
    $('#FromTime').timepicker({
        'minTime': '8:00am',
        'maxTime': '8:00pm'
    });
    //To time picker
    $('#ToTime').timepicker({
        'minTime': '8:00am',
        'maxTime': '8:00pm'
    });

    //Validation For course assigning Page
    $("#courseAssignForm").validate({
        rules: {
            DepartmentId: {
                required: true
            },
            TeacherId: {
                required: true
            },
            CourseId: {
                required: true
            }
        },
        messages: {
            DepartmentId: {
                required: "Please select an option"
            },
            TeacherId: {
                required: "Please select an option"
            },
            CourseId: {
                required: "Please select an option"
            }
        }
    });


    $("#assignCourseButton").click(function () {
        if ($("#courseAssignForm").valid()) {
            var value = (parseFloat($("#remainingCreditTextBox").val()) - parseFloat($("#TakenCourseCredit").val()));
            if (value <= 0) {

                alertify.confirm("Adding Credit", "Do you want to add more credit?",
                  function () {

                      var extraCredit = parseFloat($("#creditTobeTakenTextBox").val()) + parseFloat($("#TakenCourseCredit").val()) - parseFloat($("#remainingCreditTextBox").val());
                      $("#creditTobeTakenTextBox").val(extraCredit);
                      $("#remainingCreditTextBox").val($("#TakenCourseCredit").val());
                      addExtraCredit(extraCredit, $("#TeacherId").val());
                      alertify.set('notifier', 'position', 'top-right');
                      alertify.success(Math.abs(value) + ' Credits has been added.');
                      if ($("#courseAssignForm").valid()) {
                          var teacherId = $("#TeacherId").val();
                          var courseId = $("#CourseId").val();
                          var takenCourseCredit = $("#TakenCourseCredit").val();
                          var url = "/Admin/AssignCourseToTeacher/";

                          $.ajax({
                              url: url,
                              data: {
                                  TeacherId: teacherId,
                                  CourseId: courseId,
                                  TakenCourseCredit: takenCourseCredit
                              },
                              cache: false,
                              type: "POST",
                              success: function (data) {
                                  alertify.set('notifier', 'position', 'top-right');
                                  alertify.success(data);
                                  $("#DepartmentId").prop('selectedIndex', 0);
                                  $("#TeacherId").find('option:not(:first)').remove();
                                  $("#CourseId").find('option:not(:first)').remove();
                                  $("#creditTobeTakenTextBox").val("");
                                  $("#courseNameTextBox").val("");
                                  $("#TakenCourseCredit").val("");
                                  $("#remainingCreditTextBox").val("");
                              },
                              error: function (reponse) {
                                  alert("error-save: " + reponse);
                              }
                          });
                      }
                  },
                  function () {
                      alertify.set('notifier', 'position', 'top-right');
                      alertify.error('All have been reset.');
                      $("#DepartmentId").prop('selectedIndex', 0);
                      $("#TeacherId").find('option:not(:first)').remove();
                      $("#CourseId").find('option:not(:first)').remove();
                      $("#creditTobeTakenTextBox").val("");
                      $("#courseNameTextBox").val("");
                      $("#TakenCourseCredit").val("");
                      $("#remainingCreditTextBox").val("");
                  }).set({
                      labels: {
                          ok: "Yes",
                          cancel: "No"
                      }
                  });
            } else {
                var teacherId = $("#TeacherId").val();
                var courseId = $("#CourseId").val();
                var takenCourseCredit = $("#TakenCourseCredit").val();
                var url = "/Admin/AssignCourseToTeacher/";

                $.ajax({
                    url: url,
                    data: {
                        TeacherId: teacherId,
                        CourseId: courseId,
                        TakenCourseCredit: takenCourseCredit
                    },
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        alertify.set('notifier', 'position', 'top-right');
                        alertify.success(data);
                        $("#DepartmentId").prop('selectedIndex', 0);
                        $("#TeacherId").find('option:not(:first)').remove();
                        $("#CourseId").find('option:not(:first)').remove();
                        $("#creditTobeTakenTextBox").val("");
                        $("#courseNameTextBox").val("");
                        $("#TakenCourseCredit").val("");
                        $("#remainingCreditTextBox").val("");
                    },
                    error: function (reponse) {
                        alert("error-save: " + reponse);
                    }
                });

            }

        }
    });


    //Validation For Creationg Department Page
    $("#createDepartmentForm").validate({
        rules: {
            DepartmentCode: {
                required: true
            },
            DepartmentName: {
                required: true
            }
        },
        messages: {
            DepartmentCode: {
                required: "Please enter department code"
            },
            DepartmentName: {
                required: "Please enter department name"
            }
        }
    });

    $("#saveDepartmentButton").click(function () {
        if ($("#createDepartmentForm").valid()) {
            var departmentCode = $("#DepartmentCode").val();
            var departmentName = $("#DepartmentName").val();
            var url = "/Department/SaveDepartment/";

            $.ajax({
                url: url,
                data: {
                    DepartmentCode: departmentCode,
                    DepartmentName: departmentName
                },
                cache: false,
                type: "POST",
                success: function (data) {
                    alertify.set('notifier', 'position', 'top-right');
                    alertify.success(data);
                    $("#DepartmentCode").val("");
                    $("#DepartmentName").val("");
                },
                error: function (reponse) {
                    alert("error-save: " + reponse[0]);
                }
            });
        }


    });
    //Validating allocate form
    $("#allocateClassroomForm").validate({
        rules: {
            DepartmentId: {
                required: true
            },
            CourseId: {
                required: true
            },
            RoomId: {
                required: true
            },
            DayId: {
                required: true
            },
            FromTime: {
                required: true
            },
            ToTime: {
                required: true
            }

        },
        messages: {
            DepartmentId: {
                required: "please select an option"
            },
            CourseId: {
                required: "please select an option"
            },
            RoomId: {
                required: "please select an option"
            },
            DayId: {
                required: "please select an option"
            },
            FromTime: {
                required: "please enter the class start time"
            },
            ToTime: {
                required: "please enter the class end time"
            }
        }
    });
    //Allocating classroom
    $("#allocateButton").click(function () {
        if ($("#allocateClassroomForm").valid()) {

            var courseId = $("#CourseId").val();
            var roomId = $("#RoomId").val();
            var dayId = $("#DayId").val();
            var fromTime = $("#FromTime").val();
            var toTime = $("#ToTime").val();


            var urlAllocate = "/Admin/AllocateClassroom/";

            $.ajax({
                url: urlAllocate,
                data: {
                    CourseID: courseId,
                    RoomID: roomId,
                    DayId: dayId,
                    FromTime: fromTime,
                    ToTime: toTime
                },
                cache: false,
                type: "POST",
                success: function (data) {
                    alertify.set('notifier', 'position', 'top-right');
                    alertify.success(data);
                    $("#DepartmentId").prop('selectedIndex', 0);
                    $("#CourseId").find('option:not(:first)').remove();
                    $("#RoomId").prop('selectedIndex', 0);
                    $("#DayId").prop('selectedIndex', 0);
                    $("#FromTime").val("");
                    $("#ToTime").val("");
                },
                error: function (reponse) {
                    alert("error-save: " + reponse[0]);
                }
            });
        }
    });





    $("form[name='categoryForm']").validate({
        rules: {
            StudentId: {
                required: true
            }
        },
        messages: {
            StudentId: {
                required: "Please select an option"
            }
        }
    });

    $("#unassignButton").click(function () {
        alertify.confirm("Unassign Courses", "Are you sure to unassign all courses?",
            function () {
                var urlAllocate = "/Admin/UnassignCourses/";
                var value = 0;
                $.ajax({
                    url: urlAllocate,
                    data: {
                        code: value
                    },
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        alertify.set('notifier', 'position', 'top-right');
                        alertify.success(data);
                    },
                    error: function (reponse) {
                        alert("error-save: " + reponse[0]);
                    }
                });
            },
            function () {
                alertify.set('notifier', 'position', 'top-right');
                alertify.error("Canceled");
            }).set({
                labels: {
                    ok: "Yes",
                    cancel: "No"
                }
            });
    });

    $("#unallocateButton").click(function () {
        alertify.confirm("Unallocate Rooms", "Are you sure to unallocate all Rooms?",
            function () {
                var urlAllocate = "/Admin/UnallocateClassroom/";
                var value = 0;
                $.ajax({
                    url: urlAllocate,
                    data: {
                        code: value
                    },
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        alertify.set('notifier', 'position', 'top-right');
                        alertify.success(data);
                    },
                    error: function (reponse) {
                        alert("error-save: " + reponse[0]);
                    }
                });
            },
            function () {
                alertify.set('notifier', 'position', 'top-right');
                alertify.error("Canceled");
            }).set({
                labels: {
                    ok: "Yes",
                    cancel: "No"
                }
            });
    });
    //Sirajuls code ends from here



    //Soikat's Code Starts from Here
    //Validating course form in clientside
    $("#saveCourseForm").validate({
        rules: {
            CourseCode: {
                required: true
            },
            CourseName: {
                required: true
            },
            Credit: {
                required: true
            },
            Description: {
                required: true
            },
            DepartmentId: {
                required: true
            },
            SemesterId: {
                required: true
            }
        },
        message: {
            CourseCode: {
                required: "Please enter course code"
            },
            CourseName: {
                required: "Please enter course name"
            },
            Credit: {
                required: "Please enter credit"
            },
            Description: {
                required: "Please enter description of the course"
            },
            DepartmentId: {
                required: "Please select an option"
            },
            SemesterId: {
                required: "Please select an option"
            }
        }
    });


    //saving course with ajax
    $("#SaveCourseButton").click(function () {
        if ($("#saveCourseForm").valid()) {
            var courseCode = $("#CourseCode").val();
            var courseName = $("#CourseName").val();
            var credit = $("#Credit").val();
            var description = $("#Description").val();
            var departmentId = $("#DepartmentId").val();
            var semesterId = $("#SemesterId").val();

            var urlSaveCourse = "/Course/SaveCourse";
            $.ajax({
                url: urlSaveCourse,
                data: {
                    CourseCode: courseCode,
                    CourseName: courseName,
                    Credit: credit,
                    Description: description,
                    DepartmentId: departmentId,
                    SemesterId: semesterId
                },
                cache: false,
                type: "POST",
                success: function (data) {
                    alertify.set('notifier', 'position', 'top-right');
                    alertify.success(data);
                    $("#CourseCode").val("");
                    $("#CourseName").val("");
                    $("#Credit").val("");
                    $("#Description").val("");
                    $("#DepartmentId").prop('selectedIndex', 0);
                    $("#SemesterId").prop('selectedIndex', 0);
                },
                error: function (reponse) {
                    alert("error-Teacher-one: " + reponse.statusCode);
                }
            });
        }
    });
    //Validating teacher form in clientside
    $("#saveTeacherForm").validate({
        rules: {
            TeacherName: {
                required: true
            },
            TeacherAddress: {
                required: true
            },
            TeacherEmail: {
                required: true
            },
            TeacherContactNo: {
                required: true
            },
            DesignationId: {
                required: true
            },
            DepartmentId: {
                required: true
            },
            TakenCredit: {
                required: true
            }
        },
        message: {
            TeacherName: {
                required: "Please enter your name"
            },
            TeacherAddress: {
                required: "Please enter your address"
            },
            TeacherEmail: {
                required: "Please enter your email"
            },
            TeacherContactNo: {
                required: "Please enter your contact no"
            },
            DesignationId: {
                required: "Please select an option"
            },
            DepartmentId: {
                required: "Please select an option"
            },
            TakenCredit: {
                required: "Please enter how much credit you want to take"
            }
        }
    });


    //saving teacher with ajax
    $("#SaveTeacherButton").click(function () {
        if ($("#saveTeacherForm").valid()) {
            var teacherName = $("#TeacherName").val();
            var teacherAddress = $("#TeacherAddress").val();
            var teacherEmail = $("#TeacherEmail").val();
            var teacherContactNo = $("#TeacherContactNo").val();
            var designationId = $("#DesignationId").val();
            var departmentId = $("#DepartmentId").val();
            var takenCredit = $("#TakenCredit").val();

            var urlSaveTeacher = "/Teacher/SaveTeacher";
            $.ajax({
                url: urlSaveTeacher,
                data: {
                    TeacherName: teacherName,
                    TeacherAddress: teacherAddress,
                    TeacherEmail: teacherEmail,
                    TeacherContactNo: teacherContactNo,
                    DesignationId: designationId,
                    DepartmentId: departmentId,
                    TakenCredit: takenCredit
                },
                cache: false,
                type: "POST",
                success: function (data) {
                    alertify.set('notifier', 'position', 'top-right');
                    alertify.success(data);
                    $("#TeacherName").val("");
                    $("#TeacherAddress").val("");
                    $("#TeacherEmail").val("");
                    $("#TeacherContactNo").val("");
                    $("#DesignationId").prop('selectedIndex', 0);
                    $("#DepartmentId").prop('selectedIndex', 0);
                    $("#TakenCredit").val("");
                },
                error: function (reponse) {
                    alert("error-Teacher-one: " + reponse.statusCode);
                }
            });
        }
    });

    $("#EnrollTime").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "yy-mm-dd"

    }).datepicker("setDate", "0");

    function loadCoursesByDepartmentlId(studentId) {
        var url = "/Student/GetCoursesByDepartmentlId/";
        $.ajax({
            url: url,
            data: { studentId: studentId },
            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value=''>Select Course</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#CourseId").html(markup).show();
            },
            error: function (reponse) {
                alert("error-teacher: " + reponse);
            }
        });
    }

    $("#StudentRegEId").change(function () {
        if ($("#StudentRegEId").val() === "") {
            $("#StudentName").val("");
            $("#StudentEmail").val("");
            $("#departmentId").val("");
        }
        else {
            var studentId = $("#StudentRegEId").val();
            loadCoursesByDepartmentlId(studentId);
            var urlSaveResult = "/Student/GetStudentById/";
            var data = {
                studentId: studentId
            };
            $.ajax({
                url: urlSaveResult,
                data: JSON.stringify(data),
                cache: false,
                type: "POST",
                contentType: "application/json",
                success: function (data) {
                    $("#StudentName").val(data.StudentName);
                    $("#StudentEmail").val(data.StudentEmail);
                    $("#departmentId").val(data.Department.DepartmentName);
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                }
            });
        }

    });
    //Validating saving result form
    $("#enrollCourseForm").validate({
        rules: {
            StudentRegEId: {
                required: true
            },
            CourseId: {
                required: true
            },
            EnrollTime: {
                required: true
            }
        },
        message: {
            StudentRegEId: {
                required: "Please select an option"
            },
            CourseId: {
                required: "Please select an option"
            },
            EnrollTime: {
                required: "Please select a date"
            }
        }
    });

    //saving student result with ajax
    $("#enrollCourseButton").click(function () {
        if ($("#enrollCourseForm").valid()) {
            var studentId = $("#StudentRegEId").val();
            var courseId = $("#CourseId").val();
            var enrollTime = $("#EnrollTime").val();

            var urlEnrollStudent = "/Student/EnrollStudent/";
            $.ajax({
                url: urlEnrollStudent,
                data: {
                    StudentRegEId: studentId,
                    CourseId: courseId,
                    EnrollTime:enrollTime
                },
                cache: false,
                type: "POST",
                success: function (data) {
                    alertify.set('notifier', 'position', 'top-right');
                    alertify.success(data);
                    $("#StudentRegEId").prop('selectedIndex', 0);
                    $("#CourseId").find('option:not(:first)').remove();
                    $("#StudentName").val("");
                    $("#StudentEmail").val("");
                    $("#departmentId").val("");
                    $("#enrollTime").val("");
                },
                error: function (reponse) {
                    alert("error-Teacher-one: " + reponse.status);
                }
            });
        }
    });
    //Soikat Work ends


    //Ujjwal Work Starts Here
    $("#RegisterDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "yy-mm-dd"

    }).datepicker("setDate", "0");

    function loadCoursesByEnrollId(studentId) {
        var url = "/Student/GetCoursesByEnrollId/";
        $.ajax({
            url: url,
            data: { studentId: studentId },
            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value=''>Select Course</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#CourseId").html(markup).show();
            },
            error: function (reponse) {
                alert("error-teacher: " + reponse);
            }
        });
    }

    //Validating student form in clientside
    $("#saveStudentForm").validate({
        rules: {
            StudentName: {
                required: true
            },
            StudentEmail: {
                required: true
            },
            StudentContactNo: {
                required: true
            },
            RegisterDate: {
                required: true
            },
            DepartmentId: {
                required: true
            },
            StudentAddress: {
                required: true
            }
        },
        message: {
            StudentName: {
                required: "Please enter your name"
            },
            StudentEmail: {
                required: "Please enter your email"
            },
            StudentContactNo: {
                required: "Please enter your contact no"
            },
            RegisterDate: {
                required: "Please select an date"
            },
            DepartmentId: {
                required: "Please select an option"
            },
            StudentAddress: {
                required: "Please enter your address"
            }
        }
    });


    //saving student with ajax
    $("#SaveStudentButton").click(function () {
        if ($("#saveStudentForm").valid()) {
            var studentName = $("#StudentName").val();
            var studentEmail = $("#StudentEmail").val();
            var studentContactNo = $("#StudentContactNo").val();
            var registerDate = $("#RegisterDate").val();
            var departmentId = $("#DepartmentId").val();
            var studentAddress = $("#StudentAddress").val();

            var urlSaveStudent = "/Student/RegisterStudent";
            $.ajax({
                url: urlSaveStudent,
                data: {
                    StudentName: studentName,
                    StudentEmail: studentEmail,
                    StudentContactNo: studentContactNo,
                    RegisterDate: registerDate,
                    DepartmentId: departmentId,
                    StudentAddress: studentAddress
                },
                cache: false,
                type: "POST",
                success: function (data) {
                    alertify.set('notifier', 'position', 'top-right');
                    alertify.success(data);
                    $("#StudentName").val("");
                    $("#StudentEmail").val("");
                    $("#StudentContactNo").val("");
                    $("#RegisterDate").val("");
                    $("#DepartmentId").prop('selectedIndex', 0);
                    $("#StudentAddress").val("");
                },
                error: function (reponse) {
                    alert("error-Teacher-one: " + reponse.status);
                }
            });
        }
    });
    //Getting student info with student reg no by ajax
    $("#StudentRegId").change(function () {
        if ($("#StudentRegId").val() === "") {
            $("#StudentName").val("");
            $("#StudentEmail").val("");
            $("#departmentId").val("");
        }
        else {
            var studentId = $("#StudentRegId").val();
            loadCoursesByEnrollId(studentId);
            var urlSaveResult = "/Student/GetStudentById/";
            var data = {
                studentId: studentId
            };
            $.ajax({
                url: urlSaveResult,
                data: JSON.stringify(data),
                cache: false,
                type: "POST",
                contentType: "application/json",
                success: function (data) {
                    $("#StudentName").val(data.StudentName);
                    $("#StudentEmail").val(data.StudentEmail);
                    $("#departmentId").val(data.Department.DepartmentName);
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                }
            });
        }

    });

    //Saving Student result
    //Validating saving result form
    $("#saveResultForm").validate({
        rules: {
            StudentRegId: {
                required: true
            },
            CourseId: {
                required: true
            },
            GradeId: {
                required: true
            }
        },
        message: {
            StudentRegId: {
                required: "Please select an option"
            },
            CourseId: {
                required: "Please select an option"
            },
            GradeId: {
                required: "Please select an option"
            }
        }
    });

    //saving student result with ajax
    $("#SaveStudentResultButton").click(function () {
        if ($("#saveResultForm").valid()) {
            var studentId = $("#StudentRegId").val();
            var courseId = $("#CourseId").val();
            var gradeId = $("#GradeId").val();

            var urlCheckResult = "/Student/IsResultAlreadyExists/";
            $.ajax({
                url: urlCheckResult,
                data: {
                    studentId: studentId,
                    courseId: courseId
                },
                cache: false,
                type: "POST",
                success: function (data) {
                    if (data) {
                        alertify.confirm("Result", "Do you want to update the result?",
                             function () {
                                 var urlAllocate = "/Student/UpdateStudentResult/";
                                 var value = 0;
                                 $.ajax({
                                     url: urlAllocate,
                                     data: {
                                         StudentRegId: studentId,
                                         CourseId: courseId,
                                         GradeId: gradeId
                                     },
                                     cache: false,
                                     type: "POST",
                                     success: function (data) {
                                         alertify.set('notifier', 'position', 'top-right');
                                         alertify.success(data);
                                         $("#StudentRegId").prop('selectedIndex', 0);
                                         $("#CourseId").find('option:not(:first)').remove();
                                         $("#GradeId").prop('selectedIndex', 0);
                                         $("#StudentName").val("");
                                         $("#StudentEmail").val("");
                                         $("#RegisterDate").val("");
                                         $("#departmentId").val("");
                                     },
                                     error: function (reponse) {
                                         alert("error-save: " + reponse[0]);
                                     }
                                 });
                             },
                             function () {
                                 alertify.set('notifier', 'position', 'top-right');
                                 alertify.error("Canceled");
                                 $("#StudentRegId").prop('selectedIndex', 0);
                                 $("#CourseId").find('option:not(:first)').remove();
                                 $("#GradeId").prop('selectedIndex', 0);
                                 $("#StudentName").val("");
                                 $("#StudentEmail").val("");
                                 $("#RegisterDate").val("");
                                 $("#departmentId").val("");
                             }).set({
                                 labels: {
                                     ok: "Yes",
                                     cancel: "No"
                                 }
                             });
                    } else {
                        var urlSaveResult = "/Student/SaveStudentResult";
                        $.ajax({
                            url: urlSaveResult,
                            data: {
                                StudentRegId: studentId,
                                CourseId: courseId,
                                GradeId: gradeId
                            },
                            cache: false,
                            type: "POST",
                            success: function (data) {
                                alertify.set('notifier', 'position', 'top-right');
                                alertify.success(data);
                                $("#StudentRegId").prop('selectedIndex', 0);
                                $("#CourseId").find('option:not(:first)').remove();
                                $("#GradeId").prop('selectedIndex', 0);
                                $("#StudentName").val("");
                                $("#StudentEmail").val("");
                                $("#RegisterDate").val("");
                                $("#departmentId").val("");
                            },
                            error: function (reponse) {
                                alert("error-Teacher-one: " + reponse.status);
                            }
                        });
                    }

                },
                error: function (reponse) {
                    alert("error-Teacher-one: " + reponse.status);
                }
            });




            
        }
    });
    //Ujjwal's Code ends here
    $("#admin").hover(function() {
        $("#adminDropdown").toggle();
    });


    $("#teacher").hover(function () {
        $("#teacherDropdown").toggle();
    });

    $("#student").hover(function () {
        $("#studentDropdown").toggle();
    });


    $("#reset").hover(function () {
        $("#resetDropdown").toggle();
    });

    //$(".dropdown").hover(function () {
    //});


});