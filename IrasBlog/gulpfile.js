/// <binding AfterBuild='default' />
var gulp = require("gulp");
var uglify = require("gulp-uglify");
var concat = require("gulp-concat");

// minify Javascript
function minify() {
  return gulp.src(["wwwroot/Scripts/**/*.js"])
    .pipe(uglify())
    .pipe(concat("irasblog.min.js"))
    .pipe(gulp.dest("wwwroot/dist/"));
}

// minify CSS
function styles() {
  return gulp.src(["wwwroot/Content/**/*.css"])
    //.pipe(uglify())
    .pipe(concat("irasblog.min.css"))
    .pipe(gulp.dest("wwwroot/dist/"));
}

exports.minify = minify;

exports.styles = styles;

exports.default = minify;

// exports.default = gulp.parallel(minify, styles);
// exports.default = gulp.series(minify, styles);
