/*
Template Name: STUDIO ASP - Responsive Bootstrap 5 Admin Template
Version: 6.0.0
Author: Sean Ngu
Website: http://www.seantheme.com/studio/
*/

var lineChart, barChart, radarChart, polarAreaChart, pieChart, doughnutChart;

var handleRenderChartJs = function() {
	Chart.defaults.font.family = app.font.bodyFontFamily;
	Chart.defaults.font.size = 12;
	Chart.defaults.color = app.color.bodyColor;
	Chart.defaults.borderColor = app.color.borderColor;
	Chart.defaults.plugins.legend.display = false;
	Chart.defaults.plugins.tooltip.padding = { left: 8, right: 12, top: 8, bottom: 8 };
	Chart.defaults.plugins.tooltip.cornerRadius = 8;
	Chart.defaults.plugins.tooltip.titleMarginBottom = 6;
	Chart.defaults.plugins.tooltip.color = app.color.componentBg;
	Chart.defaults.plugins.tooltip.multiKeyBackground = app.color.componentColor;
	Chart.defaults.plugins.tooltip.backgroundColor = app.color.componentColor;
	Chart.defaults.plugins.tooltip.titleFont.family = app.font.bodyFontFamily;
	Chart.defaults.plugins.tooltip.titleFont.weight = app.font.bodyFontWeight;
	Chart.defaults.plugins.tooltip.footerFont.family = app.font.bodyFontFamily;
	Chart.defaults.plugins.tooltip.displayColors = true;
	Chart.defaults.plugins.tooltip.boxPadding = 6;
	Chart.defaults.scale.grid.color = app.color.borderColor;
	Chart.defaults.scale.beginAtZero = true;
	
	var ctx = document.getElementById('lineChart');
	lineChart = new Chart(ctx, {
		type: 'line',
		data: {
			labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
			datasets: [{
				color: app.color.theme,
				borderColor: app.color.theme,
				borderWidth: 1.5,
				pointBackgroundColor: app.color.componentBg,
				pointBorderWidth: 1.5,
				pointRadius: 4,
				pointHoverBackgroundColor: app.color.theme,
				pointHoverBorderColor: app.color.theme,
				pointHoverRadius: 7,
				label: 'Total Sales',
				data: [12, 19, 4, 5, 2, 3]
			}]
		}
	});
	
	var ctx2 = document.getElementById('barChart');
	barChart = new Chart(ctx2, {
		type: 'bar',
		data: {
			labels: ['Jan','Feb','Mar','Apr','May','Jun'],
			datasets: [{
				label: 'Total Visitors',
				data: [37,31,36,34,43,31],
				backgroundColor: 'rgba('+ app.color.themeRgb +', .25)',
				borderColor: app.color.theme,
				borderWidth: 1.5
			},{
				label: 'New Visitors',
				data: [12,16,20,14,23,21],
				backgroundColor: 'rgba('+ app.color.secondaryRgb +', .25)',
				borderColor: app.color.secondary,
				borderWidth: 1.5
			}]
		}
	});
	
	var ctx3 = document.getElementById('radarChart');
	radarChart = new Chart(ctx3, {
		type: 'radar',
		data: {
			labels: ['United States', 'Canada', 'Australia', 'Netherlands', 'Germany', 'New Zealand', 'Singapore'],
			datasets: [
				{
					label: 'Mobile',
					backgroundColor: 'rgba('+ app.color.themeRgb +', .25)',
					borderColor: app.color.theme,
					pointBackgroundColor: app.color.componentBg,
					pointBorderColor: app.color.theme,
					pointHoverBackgroundColor: app.color.componentBg,
					pointHoverBorderColor: app.color.theme,
					data: [65, 59, 90, 81, 56, 55, 40],
					borderWidth: 1.5
				},
				{
					label: 'Desktop',
					backgroundColor: 'rgba('+ app.color.secondaryRgb +', .25)',
					borderColor: app.color.secondary,
					pointBackgroundColor: app.color.componentBg,
					pointBorderColor: app.color.secondary,
					pointHoverBackgroundColor: app.color.componentBg,
					pointHoverBorderColor: app.color.secondary,
					data: [28, 48, 40, 19, 96, 27, 100],
					borderWidth: 1.5
				}
			]
		}
	});
	
	var ctx4 = document.getElementById('polarAreaChart');
	polarAreaChart = new Chart(ctx4, {
		type: 'polarArea',
		data: {
			datasets: [{
				data: [11, 16, 7, 3, 14],
				backgroundColor: ['rgba('+ app.color.themeRgb +', .5)', 'rgba('+ app.color.secondaryRgb +', .5)', 'rgba('+ app.color.themeRgb +', .25)', 'rgba('+ app.color.themeRgb +', .75)', 'rgba('+ app.color.secondaryRgb +', .75)'],
				borderWidth: 0
			}],
			labels: ['IE', 'Safari', 'Chrome', 'Firefox', 'Opera']
		}
	});
	
	var ctx5 = document.getElementById('pieChart');
	pieChart = new Chart(ctx5, {
		type: 'pie',
		data: {
			labels: ['Total Visitor', 'New Visitor', 'Returning Visitor'],
			datasets: [{
				data: [300, 50, 100],
				backgroundColor: ['rgba('+ app.color.themeRgb +', .75)', 'rgba('+ app.color.warningRgb +', .75)', 'rgba('+ app.color.successRgb +', .75)'],
				hoverBackgroundColor: ['rgba('+ app.color.themeRgb +', .5)', 'rgba('+ app.color.warningRgb +', .5)', 'rgba('+ app.color.successRgb +', .5)'],
				borderWidth: 0
			}]
		}
	});
	
	var ctx6 = document.getElementById('doughnutChart');
	doughnutChart = new Chart(ctx6, {
		type: 'doughnut',
		data: {
			labels: ['Total Visitor', 'New Visitor', 'Returning Visitor'],
			datasets: [{
				data: [300, 50, 100],
				backgroundColor: ['rgba('+ app.color.themeRgb +', .75)', 'rgba('+ app.color.themeRgb +', .25)', 'rgba('+ app.color.themeRgb +', .5)'],
				hoverBackgroundColor: [app.color.theme, app.color.theme, app.color.theme],
				borderWidth: 0
			}]
		}
	});
};

/* Controller
------------------------------------------------ */
$(document).ready(function() {
	handleRenderChartJs();
	
	$(document).on('theme-reload', function() {
		lineChart.destroy();
		barChart.destroy();
		radarChart.destroy();
		polarAreaChart.destroy();
		pieChart.destroy();
		doughnutChart.destroy();
		
		handleRenderChartJs();
	});
	
});