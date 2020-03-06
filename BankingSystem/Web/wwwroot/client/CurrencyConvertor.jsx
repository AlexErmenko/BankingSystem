class CurrencyConvertor extends React.Component {


    constructor(props) {
        super(props);
        this.state = {
            text: ''
        };
        this.handleChangeText = this.handleChangeText.bind(this);
        console.log("From comment box");
    }

    handleChangeText(e) {
        this.setState({text: e.target.value});
        console.log(e.target.value);

    }

    render() {
        return (
            <form className="commentBox">
                React render
                <input type="text"
                       value={this.state.text}
                       onChange={this.handleChangeText}/>

                <h1>{this.state.text}</h1>
            </form>
        );
    }
}

ReactDOM.render(
    <CurrencyConvertor/>,
    document.getElementById("content"),
);
