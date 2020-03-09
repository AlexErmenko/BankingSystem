"use strict";

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

var _get = function get(_x, _x2, _x3) { var _again = true; _function: while (_again) { var object = _x, property = _x2, receiver = _x3; _again = false; if (object === null) object = Function.prototype; var desc = Object.getOwnPropertyDescriptor(object, property); if (desc === undefined) { var parent = Object.getPrototypeOf(object); if (parent === null) { return undefined; } else { _x = parent; _x2 = property; _x3 = receiver; _again = true; desc = parent = undefined; continue _function; } } else if ("value" in desc) { return desc.value; } else { var getter = desc.get; if (getter === undefined) { return undefined; } return getter.call(receiver); } } };

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var CurrencyConvertor = (function (_React$Component) {
    _inherits(CurrencyConvertor, _React$Component);

    /*constructor(props) {
        super(props);
        this.state = {
            text: "",
            courses: []
        };
        this.handleChangeText = this.handleChangeText.bind(this);
        console.log("From comment box");
    }
      componentDidMount() {
        fetch("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5")
            .then(response => response.json())
            .then(console.log)
            .then(
                (response) => {
                    this.setState({courses: response.data});
                },
                (error) => {
                    console.log(error);
                }
            );
    }
      handleChangeText(e) {
        this.setState({text: e.target.value});
        console.log(e.target.value);
    }
      render() {
        return (
            <div>
                <h1>Конвертация валют</h1>
                  <div>
                    <label>В валюты </label>
                    <select className="fromCurrency">
                        <option>USD</option>
                        <option>RUB</option>
                        <option>UAH</option>
                    </select>
                    <input type="text"
                           value={this.state.text}
                           onChange={this.handleChangeText}/>
                </div>
            </div>
        );
    }*/

    function CurrencyConvertor(props) {
        _classCallCheck(this, CurrencyConvertor);

        _get(Object.getPrototypeOf(CurrencyConvertor.prototype), "constructor", this).call(this, props);

        this.handleCelsiusChange = this.handleCelsiusChange.bind(this);
        this.handleFahrenheitChange = this.handleFahrenheitChange.bind(this);

        this.state = { temperature: "", scale: "c" };
    }

    _createClass(CurrencyConvertor, [{
        key: "componentDidMount",
        value: function componentDidMount() {
            var _this = this;

            fetch("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5").then(function (response) {
                return response.json();
            }).then(console.log).then(function (response) {
                _this.setState({ courses: response.data });
            }, function (error) {
                console.log(error);
            });
        }
    }, {
        key: "handleCelsiusChange",
        value: function handleCelsiusChange(temperature) {
            this.setState({ scale: "c", temperature: temperature });
        }
    }, {
        key: "handleFahrenheitChange",
        value: function handleFahrenheitChange(temperature) {
            this.setState({ scale: "f", temperature: temperature });
        }
    }, {
        key: "render",
        value: function render() {
            var scale = this.state.scale;
            var temperature = this.state.temperature;

            var celsius = scale === "f" ? tryConvert(temperature, toCelsius) : temperature;
            var fahrenheit = scale === "c" ? tryConvert(temperature, toFahrenheit) : temperature;

            return React.createElement(
                "div",
                null,
                React.createElement(CurrencyInput, {
                    scale: "c",
                    temperature: celsius,
                    onTemperatureChange: this.handleCelsiusChange }),
                React.createElement(CurrencyInput, {
                    scale: "f",
                    temperature: fahrenheit,
                    onTemperatureChange: this.handleFahrenheitChange })
            );
        }
    }]);

    return CurrencyConvertor;
})(React.Component);

var CurrencyInput = (function (_React$Component2) {
    _inherits(CurrencyInput, _React$Component2);

    function CurrencyInput(props) {
        _classCallCheck(this, CurrencyInput);

        _get(Object.getPrototypeOf(CurrencyInput.prototype), "constructor", this).call(this, props);
        this.handleChange = this.handleChange.bind(this);
    }

    _createClass(CurrencyInput, [{
        key: "handleChange",
        value: function handleChange(e) {
            this.props.onTemperatureChange(e.target.value);
        }
    }, {
        key: "render",
        value: function render() {
            var temperature = this.props.temperature;
            var scale = this.props.scale;
            return React.createElement(
                "fieldset",
                null,
                React.createElement(
                    "legend",
                    null,
                    "Enter temperature in ",
                    scaleNames[scale],
                    ":"
                ),
                React.createElement("input", { value: temperature,
                    onChange: this.handleChange })
            );
        }
    }]);

    return CurrencyInput;
})(React.Component);

var scaleNames = {
    c: "Celsius",
    f: "Fahrenheit"
};

function toCelsius(fahrenheit) {
    return (fahrenheit - 32) * 5 / 9;
}

function toFahrenheit(celsius) {
    return celsius * 9 / 5 + 32;
}

function tryConvert(temperature, convert) {
    var input = parseFloat(temperature);
    if (Number.isNaN(input)) {
        return "";
    }
    var output = convert(input);
    var rounded = Math.round(output * 1000) / 1000;
    return rounded.toString();
}

ReactDOM.render(React.createElement(CurrencyConvertor, null), document.getElementById("content"));

