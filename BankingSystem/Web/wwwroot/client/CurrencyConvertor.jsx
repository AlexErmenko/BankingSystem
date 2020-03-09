class CurrencyConvertor extends React.Component {

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

    constructor(props) {
        super(props);

        this.handleCelsiusChange = this.handleCelsiusChange.bind(this);
        this.handleFahrenheitChange = this.handleFahrenheitChange.bind(this);

        this.state = {temperature: '', scale: 'c'};
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

    handleCelsiusChange(temperature) {
        this.setState({scale: 'c', temperature});
    }

    handleFahrenheitChange(temperature) {
        this.setState({scale: 'f', temperature});
    }

    render() {
        const scale = this.state.scale;
        const temperature = this.state.temperature;

        const celsius = scale === 'f' ? tryConvert(temperature, toCelsius) : temperature;
        const fahrenheit = scale === 'c' ? tryConvert(temperature, toFahrenheit) : temperature;

        return (
            <div>
                <CurrencyInput
                    scale="c"
                    temperature={celsius}
                    onTemperatureChange={this.handleCelsiusChange}/>
                <CurrencyInput
                    scale="f"
                    temperature={fahrenheit}
                    onTemperatureChange={this.handleFahrenheitChange}/>

            </div>
        );
    }
}

class CurrencyInput extends React.Component {

    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(e) {
        this.props.onTemperatureChange(e.target.value);
    }


    render() {
        const temperature = this.props.temperature;
        const scale = this.props.scale;
        return (
            <fieldset>
                <legend>Enter temperature in {scaleNames[scale]}:</legend>
                <input value={temperature}
                       onChange={this.handleChange}/>
            </fieldset>
        );
    }
}

const scaleNames = {
    c: 'Celsius',
    f: 'Fahrenheit'
};

function toCelsius(fahrenheit) {
    return (fahrenheit - 32) * 5 / 9;
}

function toFahrenheit(celsius) {
    return (celsius * 9 / 5) + 32;
}

function tryConvert(temperature, convert) {
    const input = parseFloat(temperature);
    if (Number.isNaN(input)) {
        return '';
    }
    const output = convert(input);
    const rounded = Math.round(output * 1000) / 1000;
    return rounded.toString();
}
ReactDOM.render(
    <CurrencyConvertor/>,
    document.getElementById("content"),
);
