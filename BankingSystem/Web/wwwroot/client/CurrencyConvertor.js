"use strict";

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

var _get = function get(_x, _x2, _x3) { var _again = true; _function: while (_again) { var object = _x, property = _x2, receiver = _x3; _again = false; if (object === null) object = Function.prototype; var desc = Object.getOwnPropertyDescriptor(object, property); if (desc === undefined) { var parent = Object.getPrototypeOf(object); if (parent === null) { return undefined; } else { _x = parent; _x2 = property; _x3 = receiver; _again = true; desc = parent = undefined; continue _function; } } else if ("value" in desc) { return desc.value; } else { var getter = desc.get; if (getter === undefined) { return undefined; } return getter.call(receiver); } } };

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var _libReact = require("../lib/react");

var _libReact2 = _interopRequireDefault(_libReact);

var CurrencyConvertor = (function (_Component) {
    _inherits(CurrencyConvertor, _Component);

    function CurrencyConvertor(props) {
        _classCallCheck(this, CurrencyConvertor);

        _get(Object.getPrototypeOf(CurrencyConvertor.prototype), "constructor", this).call(this, props);
        this.state = {
            text: '',
            courses: []
        };
        this.handleChangeText = this.handleChangeText.bind(this);
        console.log("From comment box");
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
        key: "handleChangeText",
        value: function handleChangeText(e) {
            this.setState({ text: e.target.value });
            console.log(e.target.value);
        }
    }, {
        key: "render",
        value: function render() {
            var listItem = this.state.courses.map(function (number) {
                _libReact2["default"].createElement(
                    "option",
                    null,
                    number
                );
            });
            return _libReact2["default"].createElement(
                "form",
                { className: "commentBox" },
                "React render",
                _libReact2["default"].createElement("input", { type: "text",
                    value: this.state.text,
                    onChange: this.handleChangeText }),
                _libReact2["default"].createElement(
                    "select",
                    null,
                    listItem
                ),
                _libReact2["default"].createElement(
                    "h1",
                    null,
                    this.state.text
                )
            );
        }
    }]);

    return CurrencyConvertor;
})(_libReact.Component);

ReactDOM.render(_libReact2["default"].createElement(CurrencyConvertor, null), document.getElementById("content"));

