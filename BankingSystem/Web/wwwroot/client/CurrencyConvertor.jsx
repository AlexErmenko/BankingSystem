import React , {Component} from "../lib/react";

class CurrencyConvertor extends Component {
    constructor(props) {
        super(props);
        this.state = {
            text: '',
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
                    console.log(error)
                }
            )
    }

    handleChangeText(e) {
        this.setState({text: e.target.value});
        console.log(e.target.value);
    }

    render() {
        const listItem = this.state.courses.map((number) => {
            <option>{number}</option>
        });
        return (
            <form className="commentBox">
                React render
                <input type="text"
                       value={this.state.text}
                       onChange={this.handleChangeText}/>
                       <select>
                           {listItem}
                       </select>
                <h1>{this.state.text}</h1>
            </form>
        );
    }
}

ReactDOM.render(
    <CurrencyConvertor/>,
    document.getElementById("content"),
);
