import React from 'react';
import DatePicker from 'react-datepicker';
import {rateService} from '../_services/rate.service'
import {ConvertedSum} from '../ConvertedSum';
import ru from 'date-fns/locale/ru';
import 'react-datepicker/dist/react-datepicker.css';
import 'react-datepicker/dist/react-datepicker-cssmodules.css';

import NumericInput from 'react-numeric-input';

import { userService, authenticationService } from '@/_services';

class HomePage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            currentUser: authenticationService.currentUserValue,
            date: new Date(),
            startSum: 1,
            convertedSumsResult: { }
        };
    }

    setStartDate(date)
    {
        this.setState({date: date});
    }

    setSum(sum)
    {
        this.setState({startSum: sum});
    }

    getConvertedSums(date, sum)
    {
        rateService.convertSum(date, sum)
            .then(result => this.setState({ convertedSumsResult: result }));
    }

    render() {
        const { currentUser, convertedSumsResult } = this.state;
        return (
            <div>
                <h1>Вы вошли как "{currentUser.username}".</h1>
                <div  className="form-group"><label htmlFor="sum">Сумма в рублях</label>                                
                <NumericInput 
                    className="form-control" 
                    name="sum" 
                    min={0} 
                    max={1000000000000}
                    precision={2} 
                    value={this.state.startSum} 
                    onChange={sum => this.setSum(sum)}/></div>
                <div  className="form-group"><label htmlFor="date">Дата</label>
                <DatePicker 
                    className="form-control" 
                    name="date" 
                    selected={this.state.date} 
                    onChange={date => this.setStartDate(date)}
                    dateFormat="dd.MM.yyyy"
                    maxDate={new Date()}
                    locale={ru}
                /></div>
                <div className="form-group"><button className="btn btn-primary" onClick={() => this.getConvertedSums(this.state.date, this.state.startSum)}>Конвертация</button></div>
                <div>
                    {convertedSumsResult.isSuccess &&
                        convertedSumsResult.data.map(function(item, i){
                            return <ConvertedSum key={i}
                                sum={item.sum} 
                                currencyCode={item.currencyCode} 
                                currencyTitle={item.currencyTitle} 
                                isSuccess={item.isSuccess} 
                                error={item.error}/>})
                        }
                    {convertedSumsResult.error && (<div className="alert alert-danger" role="alert">{convertedSumsResult.error}</div>)}
                </div>
            </div>
        );
    }
}

export { HomePage };