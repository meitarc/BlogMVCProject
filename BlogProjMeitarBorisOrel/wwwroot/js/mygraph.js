// Write your JavaScript code.



function start(data, id) {
    var margin = { top: 40, right: 20, bottom: 30, left: 40 },
        width = 960 - margin.left - margin.right,
        height = 500 - margin.top - margin.bottom;

    var formatPercent = d3.format(".0");

    var x = d3.scale.ordinal()
        .rangeRoundBands([0, width], .1);

    var y = d3.scale.linear()
        .range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .tickFormat(formatPercent);

    var tip = d3.tip()
        .attr('class', 'd3-tip')
        .offset([-10, 0])
        .html(function (d) {
            return "<strong>Count:</strong> <span style='color:red'>" + d.count + "</span>";
        })

    var svg = d3.select(id).append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    svg.call(tip);

    
    x.domain(data.map(function (d) {
        console.log(d);
        return d.name;
    }));
    y.domain([0, d3.max(data, function (d) {
        console.log(d);
        return d.count;
    })]);

        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(0," + height + ")")
            .call(xAxis);

        svg.append("g")
            .attr("class", "y axis")
            .call(yAxis)
            .append("text")
            .attr("transform", "rotate(-90)")
            .attr("y", 6)
            .attr("dy", ".71em")
            .style("text-anchor", "end")
            .text("Count");

    svg.selectAll(".bar")
        .data(data)
        .enter().append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return x(d.name); })
        .attr("width", x.rangeBand())
        .attr("y", function (d) { return y(d.count); })
        .attr("height", function (d) { return height - y(d.count); })
        .on('mouseover', tip.show)
        .on('mouseout', tip.hide);

    
    
}


    $.get("/Posts/Graph", {}, function (data) {
        console.log(data);
        start(data, "#mostusedcategories");
    }, "json").done(function () {
    }).fail(function (data, textStatus, xhr) {
        //This shows status code eg. 403
        console.log("error", data.status);
        console.log(data);
        console.log(xhr);
        console.log(textStatus);
        //This shows status message eg. Forbidden
        console.log("STATUS: " + xhr);
    }).always(function () {
        //TO-DO after fail/done request.
    });
