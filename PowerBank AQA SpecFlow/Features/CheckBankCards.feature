Feature: Просмотр банковских карточных продуктов

Просмотр банковский карточных продуктов

@test
Scenario Outline: Просмотр банковских карточных продуктов
	Given Открыта страница с карточными продуктами
	When Перешёл на <type> карты
	Then Произошло переключение на <type> карты
Examples:
	| type              |
	| Дебетовая карта   |
	| Кредитная карта   |
	| Виртуальная карта |
	
@test
Scenario Outline: Просмотр краткой информации по продукту
	Given Открыты <type> карточные продукты
	When Перешёл на <type> карты
	Then Краткая информация <type>, <card> соответсвует БД
Examples:
	| type              | card       |
	| Дебетовая карта   | Game Card  |
	| Кредитная карта   | Cache Card |
	| Виртуальная карта | Power Card |

@test
Scenario Outline: Просмотр подробной информации по продукту
	Given Открыты <type> карточные продукты
	When Нажал на <card>
	Then Открывается страница с <card>
	And Являющаяся <type>
	And <type>, <card> сопоставимы с базой данных
Examples:
	| type              | card          |
	| Дебетовая карта   | Solid Card    |
	| Кредитная карта   | Shopping Card |
	| Виртуальная карта | Lady Card     |


@test
Scenario Outline: Возврат к перечню продуктов
	Given Открыты <type> карточные продукты
	When Нажал на <card>
	And Нажал кнопку назад
	Then Возвращается назад на общую страницу карт с нажатой кнопкой <type>
Examples:
	| type              | card        |
	| Дебетовая карта   | Power Drive |
	| Кредитная карта   | Cache Card  |
	| Виртуальная карта | Neon Card   |

@test
Scenario Outline: Просмотр тарифов и условий выпуска
	Given Открыты <type> карточные продукты
	When Нажал на <card>
	And Выбрал тарифы по карте
	Then Открывается PDF-файл с тарифом <card>
	When Закрыл вкладку
	And Выбрал условия выпуска
	Then Открывается PDF-файл с условиями выпуска
Examples:
	| type              | card          |
	| Дебетовая карта   | Airlines      |
	| Кредитная карта   | Shopping Card |
	| Виртуальная карта | Super Card    |

